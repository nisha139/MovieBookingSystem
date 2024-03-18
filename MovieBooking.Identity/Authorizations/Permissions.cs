using System.Collections.ObjectModel;

namespace MovieBooking.Identity.Authorizations;
public class AllPermissions
{
    private static readonly Permission[] _all =
   [ 
       new("Create User",   Action.Create, Resource.Users, IsRoot: true,IsAdmin:true,IsBasic:true),
       new("Search User",   Action.Search, Resource.Users, IsRoot: true, IsAdmin: true, IsBasic: true),
       new("Edit User",     Action.Update, Resource.Users, IsRoot: true, IsAdmin: true, IsBasic: true),
       new("Delete User",   Action.Delete, Resource.Users, IsRoot: true, IsAdmin: true, IsBasic: true),
       new("Create Movie",  Action.Create, Resource.Movie, IsRoot: true),
       new("Edit Movie",    Action.Update, Resource.Movie, IsRoot: true),
       new("Search Movie",  Action.Search, Resource.Movie, IsRoot: true),
       new("Delete Movie",  Action.Delete, Resource.Movie, IsRoot: true),
       new("Create Theater",Action.Create, Resource.Theater,IsRoot:true,IsAdmin:true),
       new("Edit Theater",  Action.Update, Resource.Theater, IsRoot: true),
       new("Search Theater",Action.Search, Resource.Theater, IsRoot: true),
       new("Delete Theater",Action.Delete, Resource.Theater, IsRoot: true),
       new("Create Screen", Action.Create, Resource.Screen, IsRoot: true,IsAdmin:true),
       new("Edit Screen",   Action.Update, Resource.Screen, IsRoot: true),
       new("Search Screen ",Action.Search, Resource.Screen, IsRoot: true),
       new("Delete Screen", Action.Delete, Resource.Screen, IsRoot: true),
       new("Create Seat",   Action.Create, Resource.Seat, IsRoot: true,IsAdmin:true),
       new("Edit Seat",     Action.Update, Resource.Seat, IsRoot: true),
       new("Search Seat",   Action.Search, Resource.Seat, IsRoot: true),
       new("Delete Seat",   Action.Delete, Resource.Seat, IsRoot: true),
       new("Create Showtime", Action.Create, Resource.Showtime, IsRoot: true,IsAdmin:true),
       new("Edit Showtime",   Action.Update, Resource.Showtime, IsRoot: true),
       new("Search Showtime", Action.Search, Resource.Showtime, IsRoot: true),
       new("Delete Showtime", Action.Delete, Resource.Showtime, IsRoot: true),
       new("Create Booking",  Action.Create, Resource.Booking, IsRoot: true,IsBasic:true,IsAdmin:true),
       new("Edit Booking",    Action.Update, Resource.Booking, IsRoot: true,IsBasic:true,IsAdmin: true),
       new("Search Booking",  Action.Search, Resource.Booking, IsRoot: true),
       new("Delete Booking",  Action.Delete, Resource.Booking, IsRoot: true),
       new("Create Transaction", Action.Create, Resource.Transaction, IsRoot: true,IsBasic: true,IsAdmin: true),
       new("Edit Transaction",   Action.Update, Resource.Transaction, IsRoot: true,IsBasic: true,IsAdmin: true),
       new("Search Transaction", Action.Search, Resource.Transaction, IsRoot: true),
       new("Delete Transaction", Action.Delete, Resource.Transaction, IsRoot: true),
       new("Create PaymentMethod", Action.Create, Resource.PaymentMethod, IsRoot: true, IsBasic: true, IsAdmin: true),
       new("Edit PaymentMethod",   Action.Update, Resource.PaymentMethod, IsRoot: true, IsBasic: true, IsAdmin: true),
       new("Search PaymentMethod", Action.Search, Resource.PaymentMethod, IsRoot: true),
       new("Delete PaymentMethod", Action.Delete, Resource.PaymentMethod, IsRoot: true),
   ];

    public static IReadOnlyList<Permission> All { get; } = new ReadOnlyCollection<Permission>(_all);
    public static IReadOnlyList<Permission> Root { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.IsRoot || p.IsAdmin || p.IsBasic).ToArray());
    public static IReadOnlyList<Permission> Admin { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<Permission> User { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.IsAdmin || p.IsBasic).ToArray());

}

public record Permission(string Description, string Action, string Resource, bool IsAdmin = false, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
