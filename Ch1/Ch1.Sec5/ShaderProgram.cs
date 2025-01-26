using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Ch1.Sec5;

public class ShaderProgram
{
    private readonly int _program;

    public ShaderProgram(string vertPath, string fragPath)
    {
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
            _program = GL.CreateProgram();
            GL.AttachShader(_program, vertexShader);
            GL.AttachShader(_program, fragmentShader);
            GL.LinkProgram(_program);

            GL.GetProgram(_program, GetProgramParameterName.LinkStatus, out var success);
            if (success != 1)
            {
                var str = GL.GetProgramInfoLog(_program);
                Console.WriteLine($"Error while linking program: {str}");
            }
        }

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public void Use()
    {
        GL.UseProgram(_program);
    }

    public void SetUniform4F(string name, Vector4 value)
    {
        GL.Uniform4(GL.GetUniformLocation(_program, name), value);
    }
}