﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MadsKristensen.EditorExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebEssentialsTests
{
    [TestClass]
    public class SassCompilationTests
    {
        [ClassInitialize]
        public static void Initialize(TestContext c) { NodeExecutorBase.InUnitTests = true; }

        private static readonly string BaseDirectory = Path.GetDirectoryName(typeof(SassCompilationTests).Assembly.Location);

        [TestMethod]
        public async Task SassBasicCompilationTest()
        {
            foreach (var sourceFile in Directory.EnumerateFiles(Path.Combine(BaseDirectory, "fixtures", "sass"), "*.scss", SearchOption.AllDirectories))
            {
                var compiledFile = Path.ChangeExtension(sourceFile, ".css");

                if (!File.Exists(compiledFile))
                    continue;

                var compiled = await new SassCompiler().CompileString(File.ReadAllText(sourceFile), ".scss", ".css");
                var expected = File.ReadAllText(compiledFile)
                                   .Replace("\r", "");

                compiled.Should().Be(expected);
            }
        }
    }
}
