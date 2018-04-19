////////////////////////////////////////////////////////
/// File: Program.cs
/// Author: Daniel Probert
/// Date: 27-07-2008
/// Version: 1.0
////////////////////////////////////////////////////////
namespace DanSharp.XmlViewer
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Forms;
    using System.IO;
    using DanSharp.XmlViewer.Helpers;

    #endregion

    /// <summary>
    /// Main entry point for the application
    /// </summary>
    public static class Program
    {
        #region Public Static Methods
#if WINFORM
        /// <summary>
        /// Winform startup routine
        /// </summary>
        /// <param name="args">Arguments passed to the application</param>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
#endif

#if CONSOLE
        /// <summary>
        /// Console startup routine
        /// </summary>
        /// <param name="args">Arguments passed to the application</param>
        [STAThread]
        public static void Main(string[] args)
        {
            if ((args.Length < 2) || (args.Length > 3))
            {
                PrintUsage();
                Environment.ExitCode = 1;
                return;
            }
            else
            {
                // Get the parameters
                string action = args[0];
                string inputFile = args[1];
                string secondFile = null;
                if (args.Length == 3)
                {
                    secondFile = args[2];
                }

                // Check the action is specified correctly
                if ((!action.StartsWith("-")) || (!action.Contains(":")))
                {
                    PrintUsage();
                    Environment.ExitCode = 1;
                    return;
                }

                // Split the action into component parts
                string[] actionArray = action.Split(':');
                if (actionArray.Length != 2)
                {
                    PrintUsage();
                    Environment.ExitCode = 1;
                    return;
                }

                // Check the action is supported
                if ((string.Compare(actionArray[1], "TestCase", true) != 0)
                    && (string.Compare(actionArray[1], "XsdValidate", true) != 0))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Unknown action specified: " + actionArray[1]);
                    Console.ResetColor();
                    PrintUsage();
                    Environment.ExitCode = 1;
                    return;
                }

                // Validate input file name
                if (!FileHelper.Instance.FileExists(inputFile))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format("The Xml file '{0}' does not exist.", inputFile));
                    Console.WriteLine("Please specify a file that exists.");
                    Console.ResetColor();
                    Environment.ExitCode = 1;
                    return;
                }

                // Check if we're generating a Test Case or Validating Xsd
                if (string.Compare(actionArray[1], "TestCase", true) == 0)
                {
                    try
                    {
                        Console.Write("Generating BizUnit TestCase...");
                        // Check if we need to convert the input file name from
                        // a short name to a long name
                        if (inputFile.Contains("~"))
                        {
                            FileInfo fi = new FileInfo(inputFile);
                            inputFile = fi.FullName;
                        }
                        // Check if we need to generate the output file name
                        if (string.IsNullOrEmpty(secondFile))
                        {
                            FileInfo fi = new FileInfo(inputFile);
                            secondFile = fi.DirectoryName + "\\TestCaseFor_" + fi.Name;
                        }
                        BizUnitHelper.Instance.GenerateTestCaseAndSave(inputFile, secondFile);
                        Console.WriteLine("Done.");
                        Console.WriteLine("Successfully Generated BizUnit TestCase to file:");
                        Console.WriteLine(secondFile);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Failed.");
                        Console.WriteLine("An error occurred generating the BizUnit Test Case:");
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                        Environment.ExitCode = 1;
                        return;
                    }

                }
                else if (string.Compare(actionArray[1], "XsdValidate", true) == 0)
                {
                    // Validate xsd file name
                    if (!FileHelper.Instance.FileExists(secondFile))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(string.Format("The Xsd file '{0}' does not exist.", secondFile));
                        Console.WriteLine("Please specify a file that exists.");
                        Console.ResetColor();
                        Environment.ExitCode = 1;
                        return;
                    }

                    try
                    {
                        Console.Write("Validating Xml against Xsd...");
                        XsdValidationResult result = XsdValidationHelper.Instance.ValidateInstance(secondFile, inputFile);
                        Console.WriteLine("Done.");
                        switch (result.State)
                        {
                            case ValidationState.OtherError:
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    break;
                                }
                            case ValidationState.ValidationError:
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    break;
                                }
                            case ValidationState.Warning:
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    break;
                                }
                        }
                        Console.WriteLine(result.Results.ToString());
                        Console.ResetColor();
                        if ((result.State == ValidationState.OtherError) || (result.State == ValidationState.ValidationError))
                        {
                            Environment.ExitCode = 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Failed.");
                        Console.WriteLine("An error occurred during the validation procedure");
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                        Environment.ExitCode = 1;
                        return;
                    }
                }

            }

        }
#endif
        #endregion

        #region Private Static Methods
        /// <summary>
        /// Prints a header on the console, displayed before the results of the actions performed
        /// </summary>
        private static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("DanSharp XmlViewer");
            Console.WriteLine("BizUnit Test Case Generator and Xml Schema Validator");
            Console.WriteLine("(c) 2007 Daniel Probert, xmlviewer@probertsolutions.com");
            Console.WriteLine("http://dansharpxmlviewer.codeplex.com");
            Console.WriteLine("Version {0}.{1}.{2}", Assembly.GetEntryAssembly().GetName().Version.Major, Assembly.GetEntryAssembly().GetName().Version.Minor, Assembly.GetEntryAssembly().GetName().Version.Build);
            Console.WriteLine();
            Console.ResetColor();
        }

        /// <summary>
        /// Prints usage of the utility, if called from the console
        /// </summary>
        private static void PrintUsage()
        {
            PrintHeader();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Usage:");
            Console.WriteLine("XmlViewer.com -action:[TestCase|XsdValidate] <InputFile> <[OutputFile]|SchemaFile>");
            Console.WriteLine(@"e.g:  XmlViewer.com -action:TestCase MySample.xml MySampleTestCase.xml");
            Console.WriteLine(@"      XmlViewer.com -action:TestCase MySample.xml");
            Console.WriteLine(@"      XmlViewer.com -action:XsdValidate MySample.xml MySampleSchema.xsd");
            Console.WriteLine();
            Console.WriteLine("Note: for the TestCase action, an output file name will be generated if none is supplied.");
            Console.WriteLine();
            Console.ResetColor();
        }
        #endregion
    }
}