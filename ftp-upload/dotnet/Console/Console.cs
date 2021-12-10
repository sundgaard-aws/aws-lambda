using LambdaFunction;

public class Console {
    public static void Main(string[] args) {
        System.Console.WriteLine("Started");
        new LambdaHandler().handleRequest(null);
        System.Console.WriteLine("Ended.");
    }
}