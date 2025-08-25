using SharpDX;
using SharpNEX.Engine.EngineMath;

namespace SharpNEX.Engine.Platform.Windows.Rendering.DirectX2D1;

internal static class MatrixBuilder
{
    public static Matrix3x2 Build(Vector position, Vector size, Vector center, float angle)
    {
        var angleInRadians = Trigonometry.AngleDegreesToRadians(angle);

        var translationToOrigin = Matrix3x2.Translation(-center.X, -center.Y);
        var scaleMatrix = Matrix3x2.Scaling(size.X, size.Y);
        var rotationMatrix = Matrix3x2.Rotation(angleInRadians);
        var translationBack = Matrix3x2.Translation(center.X, center.Y);
        var translationToPosition = Matrix3x2.Translation(position.X - center.X, position.Y - center.Y);

        var combinedMatrix = translationToOrigin * scaleMatrix * rotationMatrix * translationBack * translationToPosition;
        return combinedMatrix;
    }
}