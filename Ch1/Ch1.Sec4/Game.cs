using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Ch1.Sec4;

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
            Title = "Ch1Sec4 Hello Triangle",
            Flags = ContextFlags.ForwardCompatible
        })
{
    private int _shaderProgram = 0;

    private readonly float[] _vertices =
    [
        0.5F, 0.5F, 0.0F,
        0.5F, -0.5F, 0.0F,
        -0.5F, -0.5F, 0.0F,
        -0.5F, 0.5F, 0.0F
    ];

    private readonly uint[] _indices =
    [
        0, 1, 3,
        1, 2, 3
    ];

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.Viewport(0, 0, 800, 600);

        var vertexShader = 0;
        var fragmentShader = 0;
        {
            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            var vert = File.ReadAllText("vert.glsl");
            GL.ShaderSource(vertexShader, 1, [vert], [vert.Length]);
            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out var success);
            if (success != 1)
            {
                var str = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine($"Error while compiling vert.glsl: {str}");
            }
        }

        {
            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            var frag = File.ReadAllText("frag.glsl");
            GL.ShaderSource(fragmentShader, 1, [frag], [frag.Length]);
            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out var success);
            if (success != 1)
            {
                var str = GL.GetShaderInfoLog(fragmentShader);
                Console.WriteLine($"Error while compiling frag.glsl: {str}");
            }
        }

        {
            _shaderProgram = GL.CreateProgram();
            GL.AttachShader(_shaderProgram, vertexShader);
            GL.AttachShader(_shaderProgram, fragmentShader);
            GL.LinkProgram(_shaderProgram);

            GL.GetProgram(_shaderProgram, GetProgramParameterName.LinkStatus, out var success);
            if (success != 1)
            {
                var str = GL.GetProgramInfoLog(_shaderProgram);
                Console.WriteLine($"Error while linking program: {str}");
            }
        }

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
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
        var ebo = GL.GenBuffer();

        GL.BindVertexArray(vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
            BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(float), _indices,
            BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
        GL.UseProgram(_shaderProgram);
        GL.BindVertexArray(vao);
        GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
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