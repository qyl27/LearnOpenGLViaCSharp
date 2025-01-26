using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Ch1.Sec3;

public class Game() :
    GameWindow(
        new GameWindowSettings
        {
            UpdateFrequency = 60
        },
        new NativeWindowSettings
        {
            ClientSize = new Vector2i(800, 600),
            Location = new Vector2i(200, 200),
            Title = "Ch1Sec3 Hello Window",
            Flags = ContextFlags.ForwardCompatible
        })
{
    protected override void OnLoad()
    {
        base.OnLoad();

        GL.Viewport(0, 0, 800, 600);
    }

    protected override void OnResize(ResizeEventArgs args)
    {
        base.OnResize(args);

        GL.Viewport(0, 0, args.Width, args.Height);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        GL.ClearColor(0.2F, 0.3F, 0.3F, 1.0F);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        SwapBuffers();
    }

    protected override void OnKeyUp(KeyboardKeyEventArgs args)
    {
        base.OnKeyUp(args);

        if (args.Key == Keys.Escape)
        {
            Close();
        }
    }
}