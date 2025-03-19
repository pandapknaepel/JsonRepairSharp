using JsonRepairSharp;
using static JsonRepairSharp.JsonRepair.InputType;

namespace JsonRepairSharpMinimal
{
    internal class Program
    {
        private static JsonRepair.InputType _inputType;
        private static bool _throwExceptions;

        static void Main(string[] args)
        {
            _throwExceptions = true;
            _inputType         = LLM;

            try
            {
				AssertRepair("{name: 'John'}", "{\"name\": \"John\"}");
            }
            catch (JSONRepairError err)
            {
                Console.WriteLine($"Error {err.Message} at position {err.Data["Position"]}");
            }

            Console.WriteLine("Done!");
        }


        private static void AssertRepair(string text, string expected)
        {
            string result = JsonRepair.RepairJson(text, _inputType, _throwExceptions);
            if (result == expected)
            {
                Console.WriteLine("PASS: " + text);
                Console.WriteLine("As expected: " + result);
            }
            else
            {
                Console.WriteLine("FAIL: " + text);
                Console.WriteLine("Expected: " + expected);
                Console.WriteLine("Actual: " + result);
            }
        }

        private static void AssertRepair(string text) { AssertRepair(text, text); }

    }
}