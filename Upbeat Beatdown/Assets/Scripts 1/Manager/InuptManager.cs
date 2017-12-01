using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CONTROLTYPE
{
    KEYBOARD,
    PS4,
    XBOX
}

public enum INPUTTYPE
{
    Forward,
    Backward,
    MoveLeft,
    MoveRight,
    LookX,
    LookY,
    MoveX,
    MoveY,
    Left,
    Right,
    Up,
    Down,
    Enter,
    Back,
    Escape,
    Skip,
    BassAttack,
    SnareAttack,
    Jump,

    Dash,
    AtkRight,
    AtkLeft,
    SpecialAtk,
    Shield,
    LockOn,
    Attack,
    Block
}
namespace Ins
{
    public class InuptManager : MonoBehaviour
    {
        public static CONTROLTYPE currentControls;
        private static float minAxisDash = .2f;
        private static float dashPadding = .4f;

        // check is pressed for all asigned buttons
        private static bool []buttonDownList = new bool[20];
        //private static bool isPressedX, isPressedY, bPress, buttonDwon;

        // ***************Keyboard Controls***********************
        private static Dictionary<INPUTTYPE, System.Func<bool>> PCControls =
            new Dictionary<INPUTTYPE, System.Func<bool>>
            {
                { INPUTTYPE.Forward, ()=> Input.GetKey(KeyCode.W)},
                { INPUTTYPE.Backward, ()=> Input.GetKey(KeyCode.S)},
                { INPUTTYPE.MoveLeft, ()=> Input.GetKey(KeyCode.A)},
                { INPUTTYPE.MoveRight, ()=> Input.GetKey(KeyCode.D)},
                { INPUTTYPE.Up, ()=> Input.GetKeyDown(KeyCode.UpArrow)},
                { INPUTTYPE.Down, ()=> Input.GetKeyDown(KeyCode.DownArrow)},
                { INPUTTYPE.Right, ()=> Input.GetKeyDown(KeyCode.RightArrow)},
                { INPUTTYPE.Left, ()=> Input.GetKeyDown(KeyCode.LeftArrow)},
                { INPUTTYPE.Back, ()=> Input.GetMouseButtonDown(1)},
                { INPUTTYPE.Escape, ()=> Input.GetKeyDown(KeyCode.Escape)},
                { INPUTTYPE.Skip, ()=> Input.GetKeyDown(KeyCode.Space)},

                { INPUTTYPE.Attack, () => Input.GetMouseButtonDown(0)},
                { INPUTTYPE.Block, () => Input.GetMouseButtonDown(1)},
                { INPUTTYPE.SpecialAtk, () => Input.GetKeyDown(KeyCode.LeftShift)},
                { INPUTTYPE.Dash, ()=> CheckDash(()=>Input.GetAxis("PS4_L_Analog_X"), ()=>Input.GetAxis("PS4_L_Analog_Y"), ()=>Input.GetAxis("PS4_R_Analog_X"), ()=>Input.GetAxis("PS4_R_Analog_Y"))},
                { INPUTTYPE.BassAttack, () => Input.GetKeyDown(KeyCode.Q)},
                { INPUTTYPE.SnareAttack, () => Input.GetKeyDown(KeyCode.E)},
                { INPUTTYPE.LockOn, () => Input.GetKeyDown(KeyCode.Space)}    
            };
        private static Dictionary<INPUTTYPE, System.Func<float>> PCAxis =
            new Dictionary<INPUTTYPE, System.Func<float>>
            {
                { INPUTTYPE.LookX, ()=> Input.GetAxis("Mouse X")},
                { INPUTTYPE.LookY, ()=> Input.GetAxis("Mouse Y")},

                // both WASD and arrow keys
                { INPUTTYPE.Dash, ()=> AvgAxis(() => Input.GetAxis("Horizontal"), () =>Input.GetAxis("Vertical"))},
            };
        /*private static Dictionary<INPUTTYPE, string> PCInputNames =
            new Dictionary<INPUTTYPE, string>
            {
                { INPUTTYPE.Forward, "W"},
                { INPUTTYPE.Backward, "S"},
                { INPUTTYPE.MoveRight, "D"},
                { INPUTTYPE.MoveLeft, "A"},
                { INPUTTYPE.LookX, "Mouse"},
                { INPUTTYPE.LookY, "Mouse"},
                { INPUTTYPE.Interact, "Space"},
                { INPUTTYPE.Left, "Left Arrow"},
                { INPUTTYPE.Right, "Right Arrow"},
                { INPUTTYPE.Up, "Up Arrow"},
                { INPUTTYPE.Down, "Down Arrow"},
                { INPUTTYPE.Enter, "Enter"},
                { INPUTTYPE.Back, "Space"},
                { INPUTTYPE.Escape,"Escape"}
            };*/

        // ***************PS4 Controls***********************
        private static Dictionary<INPUTTYPE, System.Func<bool>> PS4Controls =
            new Dictionary<INPUTTYPE, System.Func<bool>>
            {
                { INPUTTYPE.Forward, () => AxisToBool(() => Input.GetAxis("PS4_L_Analog_Y"), true)},    // positive
                { INPUTTYPE.Backward, () => AxisToBool(() => Input.GetAxis("PS4_L_Analog_Y"), false)},  // negative
                { INPUTTYPE.MoveLeft, () => AxisToBool(() => Input.GetAxis("PS4_L_Analog_X"), false)},
                { INPUTTYPE.MoveRight, () => AxisToBool(() => Input.GetAxis("PS4_L_Analog_X"), true)}, 
                { INPUTTYPE.Left, () => CheckFirstFrame(() => Input.GetAxis("PS4_Dpad_X"), INPUTTYPE.Left, false)},
                { INPUTTYPE.Right, () => CheckFirstFrame(() => Input.GetAxis("PS4_Dpad_X"), INPUTTYPE.Right, true)},
                { INPUTTYPE.Up, () => CheckFirstFrame(() => Input.GetAxis("PS4_Dpad_Y"), INPUTTYPE.Up, true)},            // need seperate bool for different axis
                { INPUTTYPE.Down, () =>CheckFirstFrame(() => Input.GetAxis("PS4_Dpad_Y"), INPUTTYPE.Down, false)},
                { INPUTTYPE.Escape, ()=> Input.GetButtonDown("PS4_Options")},
                { INPUTTYPE.Skip, ()=> CheckFirstFrame(() => Input.GetButtonDown("PS4_Button_Square"), INPUTTYPE.Skip)},
                //{ INPUTTYPE.Attack, () => Input.GetButtonDown("PS4_X")},
                { INPUTTYPE.Block, () => Input.GetButtonDown("PS4_Square")},
                { INPUTTYPE.SpecialAtk, () => MultAxisToBool(() => Input.GetAxis("PS4_L_Trigger"), () => Input.GetAxis("PS4_R_Trigger"))},    // positive
                { INPUTTYPE.Dash, ()=> Input.GetButtonDown("PS4_Square")},
                { INPUTTYPE.BassAttack, () => Input.GetButtonDown("PS4_L1")},
                { INPUTTYPE.SnareAttack, () => Input.GetButtonDown("PS4_R1")},
                { INPUTTYPE.LockOn, () => Input.GetButtonDown("PS4_R3")},
                { INPUTTYPE.Jump, () => Input.GetButtonDown("PS4_X")},
            };
        private static Dictionary<INPUTTYPE, System.Func<float>> PS4Axis =
            new Dictionary<INPUTTYPE, System.Func<float>>
            {
                { INPUTTYPE.LookX, ()=> Input.GetAxis("PS4_R_Analog_X")},
                { INPUTTYPE.LookY, ()=> Input.GetAxis("PS4_R_Analog_Y")},
                { INPUTTYPE.MoveX, ()=> Input.GetAxis("PS4_L_Analog_X")},
                { INPUTTYPE.MoveY, ()=> Input.GetAxis("PS4_L_Analog_Y")},

                
                { INPUTTYPE.AtkLeft, ()=> AvgAxis(() => Input.GetAxis("PS4_L_Analog_X"), () =>Input.GetAxis("PS4_L_Analog_Y"))},
                { INPUTTYPE.AtkRight, ()=> AvgAxis(() => Input.GetAxis("PS4_R_Analog_X"), () =>Input.GetAxis("PS4_R_Analog_Y"))},
            };
        /*private static Dictionary<INPUTTYPE, string> PS4InputNames =
            new Dictionary<INPUTTYPE, string>
            {
                { INPUTTYPE.Forward, "Up on the Left Analog"},
                { INPUTTYPE.Backward, "Down on the Left Analog"},
                { INPUTTYPE.MoveRight, "Right on the Left Analog"},
                { INPUTTYPE.MoveLeft, "Left on the Left Analog"},
                { INPUTTYPE.LookX, "Right on the Right Analog"},
                { INPUTTYPE.LookY, "Left on the Right Analog"},
                { INPUTTYPE.Interact, "X"},
                { INPUTTYPE.Left, "Left on the Directional Pad"},
                { INPUTTYPE.Right, "Right on the Directional Pad"},
                { INPUTTYPE.Up, "Up on the Directional Pad"},
                { INPUTTYPE.Down, "Down on the Directional Pad"},
                { INPUTTYPE.Enter, "Enter"},
                { INPUTTYPE.Back, "Circle"}
            };*/

        // ***************Xbox Controls*********************** 
        private static Dictionary<INPUTTYPE, System.Func<bool>> XboxControls =
            new Dictionary<INPUTTYPE, System.Func<bool>>
            {
                { INPUTTYPE.Forward, () => AxisToBool(() => Input.GetAxis("Xbox_L_Analog_Y"), true)},
                { INPUTTYPE.Backward, () => AxisToBool(() => Input.GetAxis("Xbox_L_Analog_Y"), false)},
                { INPUTTYPE.MoveLeft, () => AxisToBool(() => Input.GetAxis("Xbox_L_Analog_X"), false)},
                { INPUTTYPE.MoveRight, () => AxisToBool(() => Input.GetAxis("Xbox_L_Analog_X"), true)},
                { INPUTTYPE.Enter, () => Input.GetButtonDown("Xbox_Button_A")},
                { INPUTTYPE.Back, () => Input.GetButtonDown("Xbox_Button_B")},
                { INPUTTYPE.Left, () => CheckFirstFrame(() => Input.GetAxis("Xbox_Dpad_X"), INPUTTYPE.Left, false)},
                { INPUTTYPE.Right, () => CheckFirstFrame(() => Input.GetAxis("Xbox_Dpad_X"), INPUTTYPE.Right, true)},
                { INPUTTYPE.Up, () => CheckFirstFrame(() => Input.GetAxis("Xbox_Dpad_Y"), INPUTTYPE.Up, true)},
                { INPUTTYPE.Down, () =>CheckFirstFrame(() => Input.GetAxis("Xbox_Dpad_Y"), INPUTTYPE.Down, false)},
                { INPUTTYPE.Escape, ()=> Input.GetButtonDown("Xbox_Start")},
                { INPUTTYPE.Skip, ()=> CheckFirstFrame(() => Input.GetButtonDown("Xbox_Button_X"), INPUTTYPE.Skip)}
            };
        private static Dictionary<INPUTTYPE, System.Func<float>> XboxAxis =
            new Dictionary<INPUTTYPE, System.Func<float>>
            {
                { INPUTTYPE.LookX, ()=> Input.GetAxis("Xbox_R_Analog_X")},
                { INPUTTYPE.LookY, () => Input.GetAxis("Xbox_R_Analog_Y")}
            };
        /*private static Dictionary<INPUTTYPE, string> XboxInputNames =
            new Dictionary<INPUTTYPE, string>
            {
                { INPUTTYPE.Forward, "Up on the Left Analog"},
                { INPUTTYPE.Backward, "Down on the Left Analog"},
                { INPUTTYPE.MoveRight, "Right on the Left Analog"},
                { INPUTTYPE.MoveLeft, "Left on the Left Analog"},
                { INPUTTYPE.LookX, "Right on the Right Analog"},
                { INPUTTYPE.LookY, "Left on the Right Analog"},
                { INPUTTYPE.Interact, "A"},
                { INPUTTYPE.Left, "Left on the Directional Pad"},
                { INPUTTYPE.Right, "Right on the Directional Pad"},
                { INPUTTYPE.Up, "Up on the Directional Pad"},
                { INPUTTYPE.Down, "Down on the Directional Pad"},
                { INPUTTYPE.Enter, "Enter"},
                { INPUTTYPE.Back, "B"}
            };*/

        // assign control type dictionaries
        private static Dictionary<CONTROLTYPE, Dictionary<INPUTTYPE, System.Func<bool>>> Keys =
            new Dictionary<CONTROLTYPE, Dictionary<INPUTTYPE, System.Func<bool>>>
            {
                { CONTROLTYPE.KEYBOARD, PCControls},
                { CONTROLTYPE.PS4, PS4Controls},
                { CONTROLTYPE.XBOX, XboxControls}
            };
        private static Dictionary<CONTROLTYPE, Dictionary<INPUTTYPE, System.Func<float>>> Axis =
            new Dictionary<CONTROLTYPE, Dictionary<INPUTTYPE, System.Func<float>>>
            {
                { CONTROLTYPE.KEYBOARD, PCAxis},
                { CONTROLTYPE.XBOX, XboxAxis},
                { CONTROLTYPE.PS4, PS4Axis}
            };
        /*private static Dictionary<CONTROLTYPE, Dictionary<INPUTTYPE, string>> Names =
            new Dictionary<CONTROLTYPE, Dictionary<INPUTTYPE, string>>
            {
                { CONTROLTYPE.KEYBOARD, PCInputNames},
                { CONTROLTYPE.XBOX, XboxInputNames},
                { CONTROLTYPE.PS4, PS4InputNames}
            };*/

        // for tags
        private static Dictionary< CONTROLTYPE, string> controlName =
            new Dictionary< CONTROLTYPE, string>
            {
                { CONTROLTYPE.PS4, "PS4"},
                { CONTROLTYPE.XBOX, "Xbox"},
                { CONTROLTYPE.KEYBOARD, "PC"}
            };

        public static void SetControlType(CONTROLTYPE c)
        {
            currentControls = c;
        }

        public static CONTROLTYPE GetControlType()
        {
            return currentControls;
        }

        public static bool GetControls(INPUTTYPE it)
        {
            return Keys[currentControls][it].Invoke();
        }

        public static float GetAxis(INPUTTYPE it)
        {
            return Axis[currentControls][it].Invoke();
        }

        // get average axis rot for x and y of analog
        private static float AvgAxis(System.Func<float> l, System.Func<float> r)
        {
            return (l.Invoke() + r.Invoke()) +1;
        }
        
        private static bool CheckDash(System.Func<float> left_X, System.Func<float> left_Y, System.Func<float> right_X, System.Func<float> right_Y)
        {
            if (Ins.InuptManager.GetAxis(INPUTTYPE.AtkRight) != 1 && Ins.InuptManager.GetAxis(INPUTTYPE.AtkLeft) != 1 &&
                Mathf.Abs(left_X.Invoke()) + Mathf.Abs(left_Y.Invoke()) > minAxisDash && Mathf.Abs(right_X.Invoke()) + Mathf.Abs(right_Y.Invoke()) > minAxisDash &&
                Mathf.Abs(Ins.InuptManager.GetAxis(INPUTTYPE.AtkRight) - Ins.InuptManager.GetAxis(INPUTTYPE.AtkLeft)) < dashPadding )
            {
                return true;
            }
            return false;
        }

        private static bool AxisToBool(System.Func<float> f, bool isPos)
        {
            if( isPos && f.Invoke() > .1f)
            {
                return true;
            }
            else if(!isPos && f.Invoke() < -.1f)
            {
                return true;
            }
            return false;
        }

        private static bool MultAxisToBool(System.Func<float> l, System.Func<float> r)
        {
            if (l.Invoke() > -1 && r.Invoke() > -1)
            {
                return true;
            }
            return false;
        }

        public static bool GetButtonDown(System.Func<bool> f, INPUTTYPE i)
        {
            bool b = false;
            if (!buttonDownList[(int)i])
            {
                if (f.Invoke())
                {
                    buttonDownList[(int)i] = true;
                    b = true;
                }
            }
            else if(!f.Invoke())
            {
                buttonDownList[(int)i] = false;
            }

            return b;
        }

        // either button down for pc
        private static bool CheckEitherKeyDown(System.Func<bool> l, System.Func<bool> r)
        {
            if(l.Invoke() || r.Invoke())
            {
                return true;
            }
            return false;
        }
        

        private static bool CheckFirstFrame(System.Func<bool> f, INPUTTYPE i)
        {
            bool b;
            if (!buttonDownList[(int)i])
            {
                if (f.Invoke())
                {
                    //Debug.Log("first frame");
                    buttonDownList[(int)i] = true;
                    b = true;
                }
                else
                {
                    b = false;
                }
            }
            else
            {
                if (!f.Invoke())
                {
                    //Debug.Log("up frame");
                    buttonDownList[(int)i] = false;
                }
                b = false;
            }
            return b;
        }
        
        private static bool CheckFirstFrame(System.Func<float> f, INPUTTYPE i, bool isPos)
        {
            bool b;
            if (!buttonDownList[(int)i])
            {
                if (isPos && f.Invoke() > .1f)
                {
                    buttonDownList[(int)i] = true;
                    b = true;
                }
                else if (!isPos && f.Invoke() < -.1f)
                {
                    buttonDownList[(int)i] = true;
                    b = true;
                }
                else
                {
                    b = false;
                    //Debug.Log("button not pressed");
                }
            }
            else
            {
                if (f.Invoke() > -.1f && f.Invoke() < .1f)
                {
                    buttonDownList[(int)i] = false;
                    //Debug.Log("button up");
                }
                b = false;
            }
            return b;
        }

        private void Start()
        {
            // set right inputs
            currentControls = CONTROLTYPE.KEYBOARD;
            foreach (string name in Input.GetJoystickNames())
            {
                if (name == "Wireless Controller")
                {
                    currentControls = CONTROLTYPE.PS4;
                }
                else if (name.Contains("Xbox"))
                {
                    currentControls = CONTROLTYPE.XBOX;
                }
            }
            if(currentControls != CONTROLTYPE.KEYBOARD)
            {
                //Cursor.lockState = CursorLockMode.Locked;
            }

            Debug.Log(currentControls);
            //UpdateControls();
            //isPressedY = false;
            //isPressedX = false;
            //bPress = false;
        }

        private void UpdateControls()
        {
            //Debug.Log(GameObject.FindGameObjectsWithTag("PS4").Length);
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("PS4"))
            {
                //Debug.Log( g);
                if (currentControls != CONTROLTYPE.PS4)
                {
                    g.SetActive(false);
                }
                else
                {
                    g.SetActive(true);
                }
            }
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Xbox"))
            {
                if (currentControls != CONTROLTYPE.XBOX)
                {
                    g.SetActive(false);
                }
                else
                {
                    g.SetActive(true);
                }
            }
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("PC"))
            {
                if (currentControls != CONTROLTYPE.KEYBOARD)
                {
                    g.SetActive(false);
                }
                else
                {
                    g.SetActive(true);
                }
            }
        }

        public static void SetActiveButtons(GameObject g)
        {

            if (g.tag == controlName[currentControls])
            {
                g.SetActive(true);
            }
            else if(controlName.ContainsValue(g.tag))
            {
                g.SetActive(false);
            }
        }
        
    }
}