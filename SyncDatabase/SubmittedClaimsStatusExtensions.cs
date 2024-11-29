public static class SubmittedClaimsStatusExtensions
{
    public static void Print(this IReadOnlyCollection<SubmittedClaimsStatus> items)
    {
        if (!items.Any())
        {
            Console.WriteLine("No data available.");
            return; // Exit early if there are no items to print
        }

        Console.WriteLine($"Printing {items.Count} item(s):");
        foreach (SubmittedClaimsStatus item in items)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("________________________________________________________________________");
        Console.WriteLine($"Total items printed: {items.Count}");
        Console.WriteLine("________________________________________________________________________");
    }
}