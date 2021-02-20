namespace TyrolSky.Portal.Configuration {
    using System;
    using System.Runtime.InteropServices;

    public sealed class SampleConfiguration {

        public static string ConfigPath = "Sample";

        public string SampleValue { get; set; }

        public PersonInfo Person { get; set; }

        public TimeSpan SampleTimeSpan { get; set; }

    }

    public class PersonInfo {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}