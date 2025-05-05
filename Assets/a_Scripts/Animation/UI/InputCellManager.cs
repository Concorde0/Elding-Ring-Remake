using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.InputSystem
{
    public class InputCellManager
    {
        private static List<InputCell> cells = new List<InputCell>();

        public static void AddCell(InputCell cell)
        {
            cells.Add(cell);
        }

        public static void RemoveCell(InputCell cell)
        {
            cells.Remove(cell);
        }

        public static void SetAllCellsKeyTest()
        {
            foreach (InputCell cell in cells)
            {
                switch (cell.type)
                {
                    case KeyType.Key:
                        cell.SetKeyTest(InputManager.GetKeyCode(cell.keyName));
                        break;
                    case KeyType.ValueKey:
                        cell.SetKeyTest(InputManager.GetValueKeyCode(cell.keyName));
                        break;
                    case KeyType.AxisPosKey:
                        cell.SetKeyTest(InputManager.GetAxisPosCode(cell.keyName));
                        break;
                    case KeyType.AxisNegKey:
                        cell.SetKeyTest(InputManager.GetAxisNegKeyCode(cell.keyName));
                        break;
                }
            }
        }
    }
}

