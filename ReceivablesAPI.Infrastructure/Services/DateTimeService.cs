﻿using ReceivablesAPI.Application.Common.Interfaces;

namespace ReceivablesAPI.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
