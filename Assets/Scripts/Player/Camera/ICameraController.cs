

public interface ICameraController
{
    // Need to define operations that allow tools/other player components to control how the camera operates.

    // Right now, GamePlayCameraController directly consumes input. That logic should be pulled out and put somewhere else.
    // Then, that logic can utilize this interface to do the same thing.

    // Other tools can preempt control of the camera for their normal operations.

    // Camera Control Parameters

    // Camera Target
    // Minimum Following Distance
    // Maximum Following Distance
    // 
    // Orbit Distance
    // Arc
    // Rotation

    
    // The logic for default camera controller can take input and manipulate those variables.
    // The logic for ImpulseGun camera controller can do the same.

    // How?
    
    // When you use the ImpulseGun on an object, it grabs it.
    // While grabbed, the camera attempts to keep that object at the center of the screen.
    // Moving the mouse will move the camera, which will attempt to move the grabbed object.

    void OnAcquiredControl();
    void OnLostControl();
}

