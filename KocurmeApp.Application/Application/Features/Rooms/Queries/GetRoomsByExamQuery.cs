using KocurmeApp.Application.Application.Features.Rooms.DTOs;
using KocurmeApp.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace KocurmeApp.Application.Features.Rooms.Queries
{
    public record GetRoomsByExamQuery(int ExamId) : IRequest<List<RoomDTO>>;
}
