using UnityEngine;

//using functionality from "AdvancedThirdPersonController" By: Sharp Accent on YouTube.


public static class StaticVars 
{
    //Animator Parameters 
    public static string horizontal = "horizontal";
    public static string vertical = "vertical";
    public static string special = "special";
    public static string specialType = "specialType";
    public static string onLocomotion = "onLocomotion";
    public static string Horizontal = "Horizontal";
    public static string Vertical = "Vertical";
    public static string jumpType = "jumpType";
    public static string jump = "jump";
    public static string Jump = "Jump";
    public static string inAir = "inAir";
    public static string mirrorJump = "mirrorJump";
    public static string incline = "incline";


    //special animations
    public enum AnimSpecials
    {
        run, runToStop, jump_idle, run_jump
    }

    public static int getAnimSpecialType(AnimSpecials i)
    {
        int r = 0;
        switch(i)
        {
            case AnimSpecials.run:
                r = 10;
                break;
            case AnimSpecials.runToStop:
                r = 11;
                break;
            case AnimSpecials.jump_idle:
                r = 21;
                break;
            case AnimSpecials.run_jump:
                r = 22;
                break;
        }
        return r;
    }


    //fighting animations

}
