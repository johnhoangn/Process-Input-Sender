// John Nguyen
// 10.3.2021

using System;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

// 10.3.2021 
// Works with Notepad and F5(0x0074), but no other keys make it into the text editor.
// Games do not register ANY keypresses.
// Issues likely due to malformed message format.
// TODO: Using Spy++, investigate message structure of target game(s).
namespace Process_Input_Sender
{
    public static class Program
    {
		private const String NO_MATCH = "No matching process names by: {0}, or missing sleep bounds.";
		private const String NO_ARGS = "Missing process name. Exiting...";
		private const String FATAL = "ERR: {0}";
		private readonly static Random randname = new();
		private const UInt32 WM_KEYDOWN  = 0x0100;
		private const UInt32 WM_KEYUP = 0x0101;
		private const int KEY_W = 0x0057;

		[DllImport("user32.dll")]
		private static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        public static void Main(string[] args)
        {
			if (args.Length == 0) {
				Console.WriteLine(NO_ARGS);
			} else {
				Process[] apps = Process.GetProcessesByName(args[0]);
				int sleepMin = int.Parse(args[1]);
				int sleepMax = int.Parse(args[2]);
				IntPtr handle;

				try {
					handle = apps[0].MainWindowHandle;

					while (true) {
						Console.WriteLine(PostMessage(handle, WM_KEYDOWN, KEY_W, 0));
						Thread.Sleep(randname.Next(18, 31));
						Console.WriteLine(PostMessage(handle, WM_KEYUP, KEY_W, 0));
						Thread.Sleep(randname.Next(sleepMin, sleepMax));
					}
				} catch (IndexOutOfRangeException) {
					Console.WriteLine(String.Format(
						NO_MATCH,
						args[0]
					));

					foreach(Process p in Process.GetProcesses()) {
						Console.WriteLine(p.ToString());
					}
				} catch (Exception e) {
					Console.WriteLine(String.Format(
						FATAL,
						e.ToString()
					));
				}
			}
        }
    }
}
