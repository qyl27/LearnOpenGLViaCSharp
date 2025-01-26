using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Ch1.Sec5;

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
            Title = "Ch1Sec5 Hello Shaders",
            Flags = ContextFlags.ForwardCompatible
        })
{
    private ShaderProgram _shader;

    private readonly float[] _vertices =
    [
        0.5F, -0.5F, 0.0F, 1.0F, 0.0F, 0.0F,
        -0.5F, -0.5F, 0.0F, 0.0F, 1.0F, 0.0F,
        0.0F, 0.5F, 0.0F, 0.0F, 0.0F, 1.0F
    ];

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.Viewport(0, 0, 800, 600);

        _shader = new ShaderProgram("vert.glsl", "frag.glsl");
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

        var vao = GL.GenVertexArray();
        var vbo = GL.GenBuffer();

        GL.BindVertexArray(vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
            BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        _shader.Use();

        // var time = GLFW.GetTime();
        // var green = (float)(Math.Sin(time) / 2 + 0.5);
        // _shader.SetUniform4F("ourColor", new Vector4(0.0F, green, 0.0F, 1.0F));

        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        GL.BindVertexArray(0);

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