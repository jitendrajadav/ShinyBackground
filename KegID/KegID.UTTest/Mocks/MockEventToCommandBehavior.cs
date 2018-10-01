﻿using KegID.Behaviors;

namespace KegID.UTTest.Mocks
{
    public class MockEventToCommandBehavior : EventToCommandBehavior
    {
        public void RaiseEvent(params object[] args)
        {
            _handler.DynamicInvoke(args);
        }
    }
}
