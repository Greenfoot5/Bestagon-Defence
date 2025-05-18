using System;
using System.Collections.Generic;
using System.Numerics;
using Godot;
using Vector2 = Godot.Vector2;

namespace Gameplay
{
    public partial class DeathBitManager: Node2D
    {
        // If energy drops are enabled at all
        public static bool DropsEnergy = true;
        private static readonly Random Rng = new();
        // Value
        private const int NibbleValue = 2;
        private const int ByteValue = 4;
        // Scale
        private const int BitScale = 15;
        private const int NibbleScale = 27;
        private const int ByteScale = 35;
        // Position Variance
        private const float Variance = 20f;
        private const float HalfVariance = Variance * 0.5f;

        /// <summary>
        /// The Texture2D to spawn for a bit
        /// </summary>
        [Export]
        private Texture2D _bit;
        /// <summary>
        /// The Texture2D to spawn for a byte
        /// </summary>
        [Export]
        private Texture2D _nibble;
        /// <summary>
        /// The Texture2D to spawn for a byte
        /// </summary>
        [Export]
        private Texture2D _byte;
        
        // TODO - Don't do this
        /// <summary>
        /// The Texture2D to spawn for a bit
        /// </summary>
        private static Texture2D _bitS;
        /// <summary>
        /// The Texture2D to spawn for a bit
        /// </summary>
        private static Texture2D _nibbleS;
        /// <summary>
        /// The Texture2D to spawn for a byte
        /// </summary>
        private static Texture2D _byteS;

        internal static readonly List<DeathEnergy> Particles = [];

        private const float CatchRadius = 50f;
        
        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            Particles.Clear();
        }

        public override void _Ready()
        {
            _bitS = _bit;
            _nibbleS = _nibble;
            _byteS = _byte;
            Particles.Clear();

            GameStats.OnRoundProgress += CleanMap;
        }

        private void OnDestroy()
        {
            GameStats.OnRoundProgress -= CleanMap;
        }

        public static void DropEnergy(Vector2 position, int value)
        {
            if (!DropsEnergy)
            {
                GameStats.Energy += value;
                return;
            }
            
            int valueLeft = value;

            while (valueLeft > 0)
            {
                int particleValue = Rng.Next(1, Math.Min(4, valueLeft));
                
                Vector2 placePos = position + new Vector2(Variance * Rng.NextSingle() - HalfVariance, Variance * Rng.NextSingle() - HalfVariance);
                Texture2D spawnTexture = particleValue >= ByteValue ? _byteS : particleValue >= NibbleValue ? _nibbleS : _bitS;
                float scaleMultiplier = particleValue >= ByteValue ? ByteScale : particleValue >= NibbleValue ? NibbleScale : BitScale;
                
                Particles.Add(new DeathEnergy(placePos, GameStats.Rounds, Vector2.One * scaleMultiplier, particleValue, spawnTexture));
                valueLeft -= particleValue;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            var dFloat = (float)delta;
            for (var i = 0; i < Particles.Count; i++)
            {
                if (!Particles[i].Update(dFloat))
                {
                    Particles.RemoveAt(i);
                    i--;
                }
            }
            
            QueueRedraw();
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseMotion mouseMotion)
            {
                for (var i = 0; i < Particles.Count; i++)
                {
                    DeathEnergy particle = Particles[i];
                    if ((mouseMotion.GlobalPosition.DistanceSquaredTo(particle.Position)) < CatchRadius * CatchRadius)
                    {
                        GameStats.Energy += Particles[i].Value;
                        Particles[i].Collect();
                    }
                }
            }
        }

        private void CleanMap()
        {
            for (var i=0; i < Particles.Count; ++i)
            {
                if (Particles[i].StartTime <= GameStats.Rounds - 3)
                {
                    Particles[i].Collect();
                }
            }
        }

        public override void _Draw()
        {
            foreach (DeathEnergy particle in Particles)
            {
                Vector2 position = particle.Position - (particle.Scale * 0.5f);
                var rect = new Rect2(position, particle.Scale);
                DrawTextureRect(particle.Texture, rect, false);
            }
        }
    }
}