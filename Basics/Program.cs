namespace Basics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine("Press ESC key to close window");
            
            var window = new SimpleWindow();
            window.RunLife();

            Console.WriteLine("All done");

        }
    }
}
