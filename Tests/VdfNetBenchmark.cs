using System;
using System.Diagnostics;
using System.IO;
using Gameloop.Vdf;
using Newtonsoft.Json;
using SteamKit2;

namespace VdfNetBenchmark
{
    internal class VdfNetBenchmark
    {
        private static void Main()
        {
            const int numIterations = 10;
            Console.WriteLine($"Running {numIterations} iterations of deserializing the TF2 Schema...");

            string vdfStr = File.ReadAllText("tf2schema.vdf");
            string jsonStr = File.ReadAllText("tf2schema.json");

            Stopwatch sw = Stopwatch.StartNew();
            VdfNetDeserializeIterations(vdfStr, numIterations);
            sw.Stop();
            Console.WriteLine($"Vdf.NET (VDF)       : {sw.ElapsedMilliseconds/numIterations}ms, {sw.ElapsedTicks/numIterations}ticks average");

            sw = Stopwatch.StartNew();
            JsonNetDeserializeIterations(jsonStr, numIterations);
            sw.Stop();
            Console.WriteLine($"Json.NET (JSON)     : {sw.ElapsedMilliseconds/numIterations}ms, {sw.ElapsedTicks/numIterations}ticks average");

            sw = Stopwatch.StartNew();
            Sk2KeyvalueDeserializeIterations(vdfStr, numIterations);
            sw.Stop();
            Console.WriteLine($"SK2 KeyValue (VDF)  : {sw.ElapsedMilliseconds/numIterations}ms, {sw.ElapsedTicks/numIterations}ticks average");
        }

        public static void VdfNetDeserializeIterations(string vdf, int numIterations)
        {
            for (int index = 0; index < numIterations; index++)
                VdfConvert.Deserialize(vdf);
        }

        public static void JsonNetDeserializeIterations(string json, int numIterations)
        {
            for (int index = 0; index < numIterations; index++)
                JsonConvert.DeserializeObject(json);
        }

        public static void Sk2KeyvalueDeserializeIterations(string vdf, int numIterations)
        {
            for (int index = 0; index < numIterations; index++)
                KeyValue.LoadFromString(vdf);
        }
    }
}