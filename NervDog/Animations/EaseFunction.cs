using System;

namespace NervDog.Animations
{
    public abstract class EaseFunction
    {
        public static readonly NoEasing NoEasing = new NoEasing();
        public static readonly In_Out_Cubic In_Out_Cubic = new In_Out_Cubic();
        public static readonly In_Out_Quintic In_Out_Quintic = new In_Out_Quintic();
        public static readonly Out_Elastic Out_Elastic = new Out_Elastic();
        public static readonly Out_Cubic Out_Cubic = new Out_Cubic();
        public static readonly EaseG94 EaseG94 = new EaseG94();
        public static readonly In_Cubic In_Cubic = new In_Cubic();
        public static readonly In_Elastic In_Elastic = new In_Elastic();
        public static readonly Back_In_Cubic Back_In_Cubic = new Back_In_Cubic();
        public static readonly Back_Out_Cubic Back_Out_Cubic = new Back_Out_Cubic();
        public abstract float Func(float time, float begin, float change, float duration);
    }

    public class NoEasing : EaseFunction
    {
        public override float Func(float time, float begin, float change, float duration)
        {
            time /= duration;
            return begin + change*time;
        }
    }

    public class In_Out_Cubic : EaseFunction
    {
        public override float Func(float time, float begin, float change, float duration)
        {
            float ts = (time /= duration)*time;
            float tc = ts*time;
            return begin + change*(-2*tc + 3*ts);
        }
    }

    public class In_Out_Quintic : EaseFunction
    {
        public override float Func(float time, float begin, float change, float duration)
        {
            float ts = (time /= duration)*time;
            float tc = ts*time;
            return begin + change*(6*tc*ts + -15*ts*ts + 10*tc);
        }
    }

    public class Out_Elastic : EaseFunction
    {
        public override float Func(float time, float begin, float change, float duration)
        {
            float ts = (time /= duration)*time;
            float tc = ts*time;
            return begin + change*(33*tc*ts + -106*ts*ts + 126*tc + -67*ts + 15*time);
        }
    }

    public class EaseG94 : EaseFunction
    {
        public override float Func(float time, float begin, float change, float duration)
        {
            float r = 0.1f;
            return (time /= duration) < r
                ? (1 - (float) Math.Cos(time/r*Math.PI/2))*change*(1 - r) + begin
                : (float) Math.Sin((time - r)/(1 - r)*Math.PI/2)*change*r + change*(1 - r) + begin;
        }
    }

    public class Out_Cubic : EaseFunction
    {
        public override float Func(float time, float begin, float change, float duration)
        {
            float ts = (time /= duration)*time;
            float tc = ts*time;
            return begin + change*(tc + -3*ts + 3*time);
        }
    }

    public class In_Cubic : EaseFunction
    {
        public override float Func(float time, float begin, float change, float duration)
        {
            float tc = (time /= duration)*time*time;
            return begin + change*(tc);
        }
    }

    public class In_Elastic : EaseFunction
    {
        public override float Func(float time, float begin, float change, float duration)
        {
            float ts = (time /= duration)*time;
            float tc = ts*time;
            return begin + change*(33*tc*ts + -59*ts*ts + 32*tc + -5*ts);
        }
    }

    public class Back_In_Cubic : EaseFunction
    {
        public override float Func(float time, float begin, float change, float duration)
        {
            float ts = (time /= duration)*time;
            float tc = ts*time;
            return begin + change*(4*tc + -3*ts);
        }
    }

    public class Back_Out_Cubic : EaseFunction
    {
        public override float Func(float time, float begin, float change, float duration)
        {
            float ts = (time /= duration)*time;
            float tc = ts*time;
            return begin + change*(4*tc + -9*ts + 6*time);
        }
    }
}