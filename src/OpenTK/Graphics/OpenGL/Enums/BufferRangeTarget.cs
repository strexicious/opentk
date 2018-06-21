//
// The Open Toolkit Library License
//
// Copyright (c) 2006 - 2015 Stefanos Apostolopoulos for the Open Toolkit Library
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//

using System;

namespace OpenTK.Graphics.OpenGL
{
    /// <summary>
    /// Used in GL.BindBufferBase, GL.BindBufferRange and 2 other functions
    /// </summary>
    public enum BufferRangeTarget
    {
        /// <summary>
        /// Original was GL_UNIFORM_BUFFER = 0x8A11
        /// </summary>
        UniformBuffer = 0x8a11,

        /// <summary>
        /// Original was GL_TRANSFORM_FEEDBACK_BUFFER = 0x8C8E
        /// </summary>
        TransformFeedbackBuffer = 0x8c8e,

        /// <summary>
        /// Original was GL_SHADER_STORAGE_BUFFER = 0x90D2
        /// </summary>
        ShaderStorageBuffer = 0x90d2,

        /// <summary>
        /// Original was GL_ATOMIC_COUNTER_BUFFER = 0x92C0
        /// </summary>
        AtomicCounterBuffer = 0x92c0
    }
}