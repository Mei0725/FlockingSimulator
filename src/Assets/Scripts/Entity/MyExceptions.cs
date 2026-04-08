using System;

namespace MyExceptions
{
    public class NoFileFound : Exception
    {
        public NoFileFound(string path) : base($"Not find file: {path}")
        {
        }
    }

    public class MissingK : Exception
    {
        public MissingK(string name) : base($"Data missing for Weight of {name}")
        {
        }
    }

    public class ErrorK : Exception
    {
        public ErrorK(string name, float min, float max) : base($"Weight of {name} must be in [{min}, {max}]")
        {
        }
    }
    
    public class MissingN : Exception
    {
        public MissingN() : base($"Data missing for Number of Agent")
        {
        }
    }
    
    public class ErrorN : Exception
    {
        public ErrorN(int min, int max) : base($"N must be in [{min}, {max}]")
        {
        }
    }

    public class MissingT : Exception
    {
        public MissingT() : base($"Data missing for Length of Timestep")
        {
        }
    }

    public class ErrorT : Exception
    {
        public ErrorT(float min, float max) : base($"Length of Timestep must be in [{min}, {max}]")
        {
        }
    }

    public class MissingP : Exception
    {
        public MissingP() : base($"Data missing for Obstacle Center")
        {
        }
        public MissingP(string message) : base($"Data missing for Obstacle Center: {message}")
        {
        }
    }

    public class ErrorP : Exception
    {
        public ErrorP(float min, float max) : base($"Each coordinate of the Obstacle Center must be in [{min}, {max}]")
        {
        }
    }

    public class MissingR : Exception
    {
        public MissingR() : base($"Data missing for Obstacle Radius")
        {
        }
        public MissingR(string message) : base($"Data missing for Obstacle Radius: {message}")
        {
        }
    }

    public class ErrorR : Exception
    {
        public ErrorR(float min, float max) : base($"The obstacle Radius must be in [{min}, {max}]")
        {
        }
    }

    public class MissingMethod : Exception
    {
        public MissingMethod() : base($"Data missing for ODE Solver Method")
        {
        }
    }

    public class ErrorMethod : Exception
    {
        public ErrorMethod() : base($"ODE Solver Method must be in {{Euler Method, RK4, Semi-Implicit Euler Method}}")
        {
        }
    }

    public class MissingPath : Exception
    {
        public MissingPath() : base($"This function must contain the file path.")
        {
        }
    }
}
