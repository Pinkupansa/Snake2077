

public enum Direction{UP, LEFT, DOWN, RIGHT}
public enum InputMode{DEFAULT, KEYBOARD, VIRTUAL_ARROW_KEYS, VIRTUAL_JOYSTICK, FOREST_TOUCH}
public interface IInputSystem
{
    Direction GetInput();
    bool InputDetected();
}
