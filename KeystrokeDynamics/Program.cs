using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace KeystrokeDynamics
{
    static class Program
    {
        //hook type, integer value 13 indicates the hook porcedure will be installed
        private static int WH_KEYBOARD_LL = 13;
        //when the keyboard is pressed and alt(system) key isn't
        private static int WM_KEYDOWN = 0x0100;
        //represent a pointer or a handle, zero a read-only field 
        private static IntPtr hook = IntPtr.Zero;
        //callback function, systen call this function every time
        //a new keyboard event is about to be posted into a thread input queue
        private static LowLevelKeyboardProc llkProcedure = HookCallback;



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //define sethook function
            hook = SetHook(llkProcedure);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //used to remove the created hook 
            UnhookWindowsHookEx(hook);

        }


        private static String LocalTime()
        {
            //time format
            String localtime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:FFFFFFF");
            return localtime;
        }

        //public static Keys ModifierKeys { get; }

        //decalre LowLevelKeyboardProc, which delegate of the HookCallBack function(clear the error on LowLevelKeyboardProc line)
        private delegate IntPtr LowLevelKeyboardProc(int nCoden, IntPtr wParam, IntPtr lParam);

        //automatically gets called every time a key is pressed
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //determin how to process the message, if ncode is less than zero, the hook procedure must pass
            //the message to CallNextHookEx. wParam can be one of the WM_KEYDOWN, WM_KEYUP, WM_SYSKEYDOWN, WM_SYSKEYUP
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)//
            {
                /*
                 * virtual-key code(value from range 1 to254). Marshal.ReadInt32 function gets the integer
                 * value store in the memory address held in lParam, where key pressed by the user.
                 * lParam, a pointer to a KBDLLHOOKSTRUCT structure.
                */
                int vkCode = Marshal.ReadInt32(lParam);

                //measure the key pressed time
                Stopwatch sw = new Stopwatch();
                sw.Start();

                if (Control.ModifierKeys == Keys.Shift)
                {
                    SpecialCharacters(vkCode);
                }
                //determins whether the CAPS lock, num lock, or scroll locl key is in effect
                else if (!Control.IsKeyLocked(Keys.CapsLock))
                {
                    //Ascii code 65 to 90 corresponds to A to Z
                    if (vkCode >= 65 && vkCode <= 90)
                    {
                        Console.Out.Write((char)(vkCode + 32));

                        StreamWriter output = new StreamWriter(Application.StartupPath + @"\log.txt", true);
                        output.Write(LocalTime() + ": ");
                        output.Write((char)(vkCode + 32) + ": ");
                        output.Close();
                    }
                    else if (CleanOem(vkCode) == false)
                    {
                        //convert virtual key code value d1, d2 d3 to plain digit 1,2,3 etc
                        if (vkCode >= 48 && vkCode <= 57)
                        {
                            AsciiCodeLogStream(vkCode);
                        }
                        else
                        {
                            NormalLogStream(vkCode);
                        }
                    }
                }
                else if (CleanOem(vkCode) == false)
                {
                    if (vkCode >= 48 && vkCode <= 57)
                    {
                        AsciiCodeLogStream(vkCode);
                    }
                    else
                    {
                        NormalLogStream(vkCode);
                    }
                }

                sw.Stop();
                //if (((Keys)vkCode).ToString() != "LShiftKey")
                //{
                StreamWriter write = new StreamWriter(Application.StartupPath + @"\log.txt", true);
                write.Write(sw.Elapsed + ": " + LocalTime() + Environment.NewLine);
                write.Close();
                //}
            }

            //hook procedure are installe in chians for particulat hook types,
            //CallNextHookEx calls the next hook in the chain
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }



        private static void NormalLogStream(int vkCode)
        {
            /*
            * keys is used to convert the integer value of the key pressed into human readable format,
            * then can write that value put to the command-line or log file. 
            */
            Console.Out.Write((Keys)vkCode);
            StreamWriter output = new StreamWriter(Application.StartupPath + @"\log.txt", true);
            output.Write(LocalTime() + ": ");
            output.Write((Keys)vkCode + ": ");
            output.Close();

        }

        //write cleaned OEM encoding 
        private static void OemLogStream(String cleanEnco)
        {
            Console.Out.Write(cleanEnco);

            //write logged key into text file, true allows then code to append to the file alread exists
            StreamWriter output = new StreamWriter(Application.StartupPath + @"\log.txt", true);
            //writre current pc time
            output.Write(LocalTime() + ": ");
            output.Write(cleanEnco + ": ");
            output.Close();
        }

        //convert to Ascii symbol
        private static void AsciiCodeLogStream(int vkCode)
        {
            Console.Out.Write((char)vkCode);

            StreamWriter output = new StreamWriter(Application.StartupPath + @"\log.txt", true);
            output.Write(LocalTime() + ": ");
            output.Write((char)vkCode + ": ");
            output.Close();
        }

        //convert Oem virtual key code to special character
        private static void SpecChLogStream(char ch)
        {
            //convert ascii code(vkCode number) to character/symbol
            Console.Out.Write(GetModifiedKey(ch));

            StreamWriter output = new StreamWriter(Application.StartupPath + @"\log.txt", true);
            output.Write(LocalTime() + ": ");
            output.Write(GetModifiedKey(ch) + ": ");
            output.Close();
        }

        //convert virtual key code to special character
        private static void SpecialCharacters(int vkCode)
        {
            //convert vitrual key code to string
            string vkStringValue = ((Keys)vkCode).ToString();

            //if (Keys)vkCode) string value is equal to Oemplus, translate it to keyboard correspon special character
            if (vkStringValue == "Oemplus")
            {
                //LogFileStream(Keys.D1);
                SpecChLogStream('+');
            }
            else if (vkStringValue == "OemMinus")
            {
                SpecChLogStream('-');
            }
            else if (vkStringValue == "OemOpenBrackets")
            {
                SpecChLogStream('[');
            }
            else if (vkStringValue == "Oem6")
            {
                SpecChLogStream(']');
            }
            else if (vkStringValue == "Oem1")
            {
                SpecChLogStream(';');
            }
            else if (vkStringValue == "Oemtilde")
            {
                SpecChLogStream('\'');
            }
            else if (vkStringValue == "Oem7")
            {
                SpecChLogStream('#');
            }
            else if (vkStringValue == "OemPeriod")
            {
                SpecChLogStream(',');
            }
            else if (vkStringValue == "Oemcomma")
            {
                SpecChLogStream('.');
            }
            else if (vkStringValue == "OemQuestion")
            {
                SpecChLogStream('/');
            }
            else if (vkStringValue == "Oem5")
            {
                SpecChLogStream('\\');
            }
            else if (vkStringValue == "Oem8")
            {
                SpecChLogStream('`');
            }

            else if (vkStringValue == "LShiftKey")
            {
                NormalLogStream(vkCode);
            }
            else
            {
                //output on console 
                Console.Out.Write(GetModifiedKey(Convert.ToChar((Keys)vkCode)));
                //write into text file
                StreamWriter output = new StreamWriter(Application.StartupPath + @"\log.txt", true);
                output.Write(LocalTime() + ": ");
                output.Write(GetModifiedKey(Convert.ToChar(vkCode)) + ":");
                output.Close();
            }
        }

        // clean up the encoding for Oem virtual key code
        private static bool CleanOem(int vkCode)
        {

            //clean up the encoding for period(.), comma(,), sapce etc.
            if (((Keys)vkCode).ToString() == "OemPeriod")////convert interger value vkCode into string
            {
                /*Console.Out.Write(" ");

                StreamWriter output = new StreamWriter(Application.StartupPath + @"\log.txt", true);
                output.Write(LocalTime() + ": ");
                output.Write(" " + ": ");
                output.Close(); */
                OemLogStream(".");
                return true;
            }
            else if (((Keys)vkCode).ToString() == "Oemcomma")
            {
                OemLogStream(",");
                return true;
            }
            else if (((Keys)vkCode).ToString() == "Space")
            {
                OemLogStream(" ");
                return true;
            }
            else if (((Keys)vkCode).ToString() == "Oemplus")
            {
                OemLogStream("=");
                return true;
            }
            else if (((Keys)vkCode).ToString() == "OemMinus")
            {
                OemLogStream("-");
                return true;
            }
            else if (((Keys)vkCode).ToString() == "OemOpenBrackets")
            {
                OemLogStream("[");
                return true;
            }
            else if (((Keys)vkCode).ToString() == "Oem6")
            {
                OemLogStream("]");
                return true;
            }
            else if (((Keys)vkCode).ToString() == "Oem1")
            {
                OemLogStream(";");
                return true;
            }
            else if (((Keys)vkCode).ToString() == "Oemtilde")
            {
                OemLogStream("'");
                return true;
            }
            else if (((Keys)vkCode).ToString() == "Oem7")
            {
                OemLogStream("#");
                return true;
            }
            else if (((Keys)vkCode).ToString() == "OemQuestion")
            {
                OemLogStream("/");
                return true;
            }
            else if (((Keys)vkCode).ToString() == "Oem5")
            {
                OemLogStream("\\");
                return true;
            }
            else if (((Keys)vkCode).ToString() == "Oem8")
            {
                OemLogStream("`");
                return true;
            }
            else
            {
                return false;
            }
        }

        //define sethook function and build and return the SetWindowsHookEx function
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            Process currentProcess = Process.GetCurrentProcess();
            ProcessModule currentModule = currentProcess.MainModule;
            String moduleName = currentModule.ModuleName;
            IntPtr moduleHandle = GetModuleHandle(moduleName);

            return SetWindowsHookEx(WH_KEYBOARD_LL, llkProcedure, moduleHandle, 0);
        }

        public static char GetModifiedKey(char c)
        {
            //translate the cahrater into a virtual key, if the virtual key value is less than 1, error occuring
            short vkKeyScanResult = VkKeyScan(c);

            //a result of -1 indicates no key translates to input character
            if (vkKeyScanResult == -1)
            {
                throw new ArgumentException("No key mapping for " + c);
            }

            //vkKeyScanResult & 0xff is the base key, without any modifiers
            uint code = (uint)vkKeyScanResult & 0xff;

            //set shift key pressed
            byte[] b = new byte[256];
            //virtual key code simulates the shift key being pressed https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
            //b[0x10] is the shift virtual key code in heximal?
            b[0x10] = 0x80;

            uint r;

            //return value of 1 expected (1 character copied to r)
            if (1 != ToAscii(code, code, b, out r, 0))
            {
                throw new ApplicationException("Could not translate modified state");
            }
            return (char)r;
        }


        //User32.dll is the source of many of the most common Windows API's, that allow program to interact with the operating system at a lower level
        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParan, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idhook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        //kernel32.dll it handles memory management, is loaded into a protected memory space so other applications do not take that space over
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(String lpMoudleName);

        [DllImport("user32.dll")]
        static extern short VkKeyScan(char c);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int ToAscii(uint uVirtKey, uint uScanCode, byte[] lpKeyState, out uint lpChar, uint flags);
    }

}

