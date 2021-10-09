using System;
using System.Threading.Tasks;
using NEOSimulation.Components.Orbital;
using NEOSimulation.Entities;
using NEOSimulation.Types;
using Nez;

namespace NEOSimulation.Components
{
    public class TimeManager : Component, IUpdatable
    {
        public DateTime CurrentDate;

        public TimePlayDirection CurrentPlayDirection = TimePlayDirection.None;
        public TimeStep CurrentTimeStep = TimeStep.Hour;

        private DateTime J2000 = new DateTime(2000, 1, 1, 12, 0, 0);

        public void InitializePositions()
        {
            CurrentDate = J2000;
            ChangeDate(CurrentDate);
        }

        public override void OnAddedToEntity()
        {
            InitializePositions();
            
            base.OnAddedToEntity();
        }

        public void ChangeDate(DateTime newDate)
        {
            CurrentDate = newDate;
            
            var bodies = MainScene.Instance.BodyArray;
            
            Parallel.For(0, bodies.Length, i =>
            {
                bodies[i].GetComponent<Body>().ChangePositionToEpoch(newDate);
            }); 
        }

        public void Update()
        {
            switch (CurrentPlayDirection)
            {
                case TimePlayDirection.Backward:
                    StepBack();
                    break;
                case TimePlayDirection.Forward:
                    StepForward();
                    break;
            }
        }

        public void StepForward()
        {
            int totalHoursForward = TotalHoursCurrentStep();
            ProgressDate(totalHoursForward);
        }
        
        public void StepBack()
        {
            int totalHoursBackward = TotalHoursCurrentStep();
            RegressDate(totalHoursBackward);
        }

        private void ProgressDate(int totalHours)
        {
            ChangeDate(CurrentDate.AddHours(totalHours));
        }
        
        private void RegressDate(int totalHours)
        {
            ChangeDate(CurrentDate.AddHours(-totalHours));
        }

        private int TotalHoursCurrentStep()
        {
            int totalHours = 0;
            switch (CurrentTimeStep)
            {
                case TimeStep.Hour:
                    totalHours = 1;
                    break;
                case TimeStep.SixHours:
                    totalHours = 6;
                    break;
                case TimeStep.Day:
                    totalHours = 24;
                    break;
                case TimeStep.Week:
                    totalHours = 24 * 7;
                    break;
                case TimeStep.Month:
                    totalHours = 24 * 7 * 30;
                    break;
                case TimeStep.SixMonths:
                    totalHours = 24 * 7 * 30 * 6;
                    break;
                case TimeStep.Year:
                    totalHours = 24 * 7 * 30 * 12;
                    break;
            }

            return totalHours;
        }
    }
}