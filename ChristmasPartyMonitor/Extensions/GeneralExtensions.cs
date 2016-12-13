﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.ProjectOxford.Common;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Face.Contract;

namespace ChristmasPartyMonitor.Extensions
{
    public static class GeneralExtensions
    {
        public static bool FaceRectanglesAreEqual(this FaceRectangle faceRectangle, Rectangle rectangle)
        {
            return faceRectangle.Height == rectangle.Height && faceRectangle.Width == rectangle.Width &&
                   faceRectangle.Left == rectangle.Left && faceRectangle.Top == rectangle.Top;
        }

        public static KeyValuePair<string, float> GetEmotion(this Scores scores)
        {
            return scores.ToRankedList().FirstOrDefault();
        }
    }
}