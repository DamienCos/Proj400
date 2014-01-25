using Microsoft.Xna.Framework;

namespace MonoGameConversionHelper
{
    /// <summary>
    /// Helps convert between XNA math types and the BEPUphysics replacement math types.
    /// A version of this converter could be created for other platforms to ease the integration of the engine.
    /// </summary>
    public static class MathConverter
    {
        //Vector2
      

        public static BEPUutilities.Vector2 Convert(Vector2 xnaVector)
        {
            BEPUutilities.Vector2 toReturn;
            toReturn.X = xnaVector.X;
            toReturn.Y = xnaVector.Y;
            return toReturn;
        }

        public static void Convert(ref Vector2 xnaVector, out BEPUutilities.Vector2 bepuVector)
        {
            bepuVector.X = xnaVector.X;
            bepuVector.Y = xnaVector.Y;
        }

        //Vector3
        

        public static BEPUutilities.Vector3 Convert(Vector3 xnaVector)
        {
            BEPUutilities.Vector3 toReturn;
            toReturn.X = xnaVector.X;
            toReturn.Y = xnaVector.Y;
            toReturn.Z = xnaVector.Z;
            return toReturn;
        }

        public static void Convert(ref Vector3 xnaVector, out BEPUutilities.Vector3 bepuVector)
        {
            bepuVector.X = xnaVector.X;
            bepuVector.Y = xnaVector.Y;
            bepuVector.Z = xnaVector.Z;
        }

       
        public static BEPUutilities.Vector3[] Convert(Vector3[] xnaVectors)
        {
            var bepuVectors = new BEPUutilities.Vector3[xnaVectors.Length];
            for (int i = 0; i < xnaVectors.Length; i++)
            {
                Convert(ref xnaVectors[i], out bepuVectors[i]);
            }
            return bepuVectors;

        }

        //Matrix
      

        public static BEPUutilities.Matrix Convert(Matrix matrix)
        {
            BEPUutilities.Matrix toReturn;
            Convert(ref matrix, out toReturn);
            return toReturn;
        }

       

        public static void Convert(ref Matrix matrix, out BEPUutilities.Matrix bepuMatrix)
        {
            bepuMatrix.M11 = matrix.M11;
            bepuMatrix.M12 = matrix.M12;
            bepuMatrix.M13 = matrix.M13;
            bepuMatrix.M14 = matrix.M14;

            bepuMatrix.M21 = matrix.M21;
            bepuMatrix.M22 = matrix.M22;
            bepuMatrix.M23 = matrix.M23;
            bepuMatrix.M24 = matrix.M24;

            bepuMatrix.M31 = matrix.M31;
            bepuMatrix.M32 = matrix.M32;
            bepuMatrix.M33 = matrix.M33;
            bepuMatrix.M34 = matrix.M34;

            bepuMatrix.M41 = matrix.M41;
            bepuMatrix.M42 = matrix.M42;
            bepuMatrix.M43 = matrix.M43;
            bepuMatrix.M44 = matrix.M44;

        }

      

     

        //Quaternion
      

        public static BEPUutilities.Quaternion Convert(Quaternion quaternion)
        {
            BEPUutilities.Quaternion toReturn;
            toReturn.X = quaternion.X;
            toReturn.Y = quaternion.Y;
            toReturn.Z = quaternion.Z;
            toReturn.W = quaternion.W;
            return toReturn;
        }

       
        public static void Convert(ref Quaternion quaternion, out  BEPUutilities.Quaternion bepuQuaternion)
        {
            bepuQuaternion.X = quaternion.X;
            bepuQuaternion.Y = quaternion.Y;
            bepuQuaternion.Z = quaternion.Z;
            bepuQuaternion.W = quaternion.W;
        }

        //Ray
        public static BEPUutilities.Ray Convert(Ray ray)
        {
            BEPUutilities.Ray toReturn;
            Convert(ref ray.Position, out toReturn.Position);
            Convert(ref ray.Direction, out toReturn.Direction);
            return toReturn;
        }

        public static void Convert(ref Ray ray, out BEPUutilities.Ray bepuRay)
        {
            Convert(ref ray.Position, out bepuRay.Position);
            Convert(ref ray.Direction, out bepuRay.Direction);
        }

       

        //BoundingBox
        

        public static BEPUutilities.BoundingBox Convert(BoundingBox boundingBox)
        {
            BEPUutilities.BoundingBox toReturn;
            Convert(ref boundingBox.Min, out toReturn.Min);
            Convert(ref boundingBox.Max, out toReturn.Max);
            return toReturn;
        }

       

        public static void Convert(ref BoundingBox boundingBox, out BEPUutilities.BoundingBox bepuBoundingBox)
        {
            Convert(ref boundingBox.Min, out bepuBoundingBox.Min);
            Convert(ref boundingBox.Max, out bepuBoundingBox.Max);
        }

        //BoundingSphere
      

        public static BEPUutilities.BoundingSphere Convert(BoundingSphere boundingSphere)
        {
            BEPUutilities.BoundingSphere toReturn;
            Convert(ref boundingSphere.Center, out toReturn.Center);
            toReturn.Radius = boundingSphere.Radius;
            return toReturn;
        }

    
        public static void Convert(ref BoundingSphere boundingSphere, out BEPUutilities.BoundingSphere bepuBoundingSphere)
        {
            Convert(ref boundingSphere.Center, out bepuBoundingSphere.Center);
            bepuBoundingSphere.Radius = boundingSphere.Radius;
        }

        //Plane
      

        public static BEPUutilities.Plane Convert(Plane plane)
        {
            BEPUutilities.Plane toReturn;
            Convert(ref plane.Normal, out toReturn.Normal);
            toReturn.D = plane.D;
            return toReturn;
        }

       
        public static void Convert(ref Plane plane, out BEPUutilities.Plane bepuPlane)
        {
            Convert(ref plane.Normal, out bepuPlane.Normal);
            bepuPlane.D = plane.D;
        }

        public static Microsoft.Xna.Framework.Matrix ConvertToXna(Matrix matrix)
        {
            Microsoft.Xna.Framework.Matrix xnaMatrix = new Microsoft.Xna.Framework.Matrix();

            xnaMatrix.M11 = matrix.M11;
            xnaMatrix.M12 = matrix.M12;
            xnaMatrix.M13 = matrix.M13;
            xnaMatrix.M14 = matrix.M14;

            xnaMatrix.M21 = matrix.M21;
            xnaMatrix.M22 = matrix.M22;
            xnaMatrix.M23 = matrix.M23;
            xnaMatrix.M24 = matrix.M24;

            xnaMatrix.M31 = matrix.M31;
            xnaMatrix.M32 = matrix.M32;
            xnaMatrix.M33 = matrix.M33;
            xnaMatrix.M34 = matrix.M34;

            xnaMatrix.M41 = matrix.M41;
            xnaMatrix.M42 = matrix.M42;
            xnaMatrix.M43 = matrix.M43;
            xnaMatrix.M44 = matrix.M44;

            return (xnaMatrix);
        }
    }
}

