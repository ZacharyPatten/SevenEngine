using System;

using OpenTK.Input;

using Engine.Mathematics;

namespace Engine
{
  public class Mouse
  {
    MouseDevice _mouse;

    public Mouse(MouseDevice mouse) { _mouse = mouse; }

    //public void Update()
    //{
    //  UpdateMousePosition();
    //  UpdateMouseButtons();
    //}

    //private void UpdateMousePosition()
    //{
    //  System.Drawing.Point mousePos = Cursor.Position;
    //  mousePos = _openGLControl.PointToClient(mousePos);

    //  // Now use our point definition, 
    //  Engine.Mathematics.Point adjustedMousePoint = new Engine.Mathematics.Point();
    //  adjustedMousePoint.X = (float)mousePos.X - ((float)_parentForm.ClientSize.Width / 2);
    //  adjustedMousePoint.Y = ((float)_parentForm.ClientSize.Height / 2) - (float)mousePos.Y;
    //  Position = adjustedMousePoint;
    //}

    //private void UpdateMouseButtons()
    //{
    //  // Reset buttons
    //  MiddlePressed = false;
    //  LeftPressed = false;
    //  RightPressed = false;

    //  if (_leftClickDetect)
    //  {
    //    LeftPressed = true;
    //    _leftClickDetect = false;
    //  }

    //  if (_rightClickDetect)
    //  {
    //    RightPressed = true;
    //    _rightClickDetect = false;
    //  }

    //  if (_middleClickDetect)
    //  {
    //    MiddlePressed = true;
    //    _middleClickDetect = false;
    //  }
    //}
  }
}