using UnityEngine;
using System;

public class InputController : MonoBehaviour
{
    static Movement ToMovement(int x, int y)
    {
        // only allow 1 direction, prioritize horizontal over vertical
        if (x != 0)
        {
            return x > 0 ? Movement.Right : Movement.Left;
        }
        else if (y != 0)
        {
            return y > 0 ? Movement.Up : Movement.Down;
        }
        throw new ArgumentException(String.Format(
            "Can't translate x:{0} y:{1} to movement.", x, y));
    }

    static Vector2 GetKBInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"),
                           Input.GetAxisRaw("Vertical"));
    }

    Vector2 touchOrigin = -Vector2.one;

    void Update()
    {
        var moveVector = GetInput();
        int horizontal = Mathf.RoundToInt(moveVector.x);
        int vertical = Mathf.RoundToInt(moveVector.y);

        if (horizontal != 0 || vertical != 0)
        {
            var movement = ToMovement(horizontal, vertical);
            var pool = Contexts.sharedInstance.pool;
            pool.ReplaceMoveInput(movement);
            pool.moveInputEntity.isDeleteOnExit = true;
        }
    }

    Vector2 GetInput()
    {
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        return GetTouchInput();
#else
        return GetKBInput();
#endif
    }

    Vector2 GetTouchInput()
    {
        //Check if Input has registered more than zero touches
        if (Input.touchCount <= 0)
        {
            return Vector2.zero;
        }

        int horizontal = 0;
        int vertical = 0;

        Touch firstTouch = Input.touches[0];

        //Check if the phase of that touch equals Began
        if (firstTouch.phase == TouchPhase.Began)
        {
            //If so, store position of that touch for later calculations
            touchOrigin = firstTouch.position;
        }
        else if (firstTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
        {
            Vector2 touchEnd = firstTouch.position;
            float x = touchEnd.x - touchOrigin.x;
            float y = touchEnd.y - touchOrigin.y;

            // Set touchOrigin.x to -1 so that our else if statement will 
            // evaluate false and not repeat immediately.
            touchOrigin.x = -1;

            // Check if the difference along the x axis is greater than the
            // difference along the y axis.
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                horizontal = x > 0 ? 1 : -1;
            }
            else
            {
                vertical = y > 0 ? 1 : -1;
            }
        }

        return new Vector2(horizontal, vertical);
    }
}
