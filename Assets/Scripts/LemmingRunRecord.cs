﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace
{
    [Serializable]
    public class LemmingRunRecord
    {
        [SerializeField]
        private List<LemmingMovementDirection> _data = new List<LemmingMovementDirection>();

        public LemmingRunRecord()
        {
            _data = new List<LemmingMovementDirection>();
        }

        public LemmingRunRecord(LemmingMovementDirection[] data)
        {
            _data = new List<LemmingMovementDirection>(data);
        }

        public LemmingMovementDirection GetOrGenerateNextMovement(int currentStep)
        {
            if (_data.Count <= currentStep)
            {
                while (_data.Count <= currentStep)
                {
                    _data.Add(LemmingUtils.GenerateNextDirection());    
                }
            }

            return _data[currentStep];
        }

        public void AddMovement(LemmingMovementDirection direciton)
        {
            _data.Add(direciton);
        }

        public int Size
        {
            get { return _data.Count; }
        }

        public LemmingMovementDirection this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }
        
        public void Mutate(double mutationRatio = 0.1)
        {
            var numberOfMutations = (int) (Size * mutationRatio);
            var random = new Random();
            for (var i = 0; i < numberOfMutations; i++)
                _data[(int) (LemmingUtils.GetRandom(random) * Size)] = LemmingUtils.GenerateNextDirection();
        }

        public void MutateLastActions(int killPoint)
        {
            var numberOfMutations = new Random().Next(30, 100);
            var mutatePoint = Math.Max(0, killPoint - numberOfMutations + 1);
            if(killPoint != _data.Count)
            {
                Debug.Log(string.Format("Mutate actions from {0} to {1}, remove actions from {2} to {3} "
                    , mutatePoint, killPoint - 1, killPoint, _data.Count - killPoint - 1));

                _data.RemoveRange(killPoint, _data.Count - killPoint - 1);
                for (var i = mutatePoint; i < killPoint; i++)
                    _data[i] = LemmingUtils.GenerateNextDirection();
            }
        }
    }
}