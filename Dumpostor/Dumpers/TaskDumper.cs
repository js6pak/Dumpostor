using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;

namespace Dumpostor.Dumpers
{
    public class TaskDumper : IMapDumper
    {
        public string FileName => "tasks";

        public string Dump(MapTypes mapType, ShipStatus shipStatus)
        {
            var consoles = shipStatus.GetComponentsInChildren<Console>();

            var tasks = new Dictionary<int, object>();

            foreach (var taskType in EnumExtensions.GetValues<TaskTypes>())
            {
                var taskConsoles = new List<TaskConsole>();

                foreach (var console in consoles.Where(x => x.TaskTypes.Contains(taskType)))
                {
                    taskConsoles.Add(new TaskConsole(taskType, console.Room, console.transform.position, console.UsableDistance));
                }

                if (!taskConsoles.Any())
                {
                    continue; // current map doesn't have this task at all
                }

                var taskLength = GetTaskLength(shipStatus, taskType);
                if (taskLength == null)
                {
                    continue;
                }

                tasks.Add((int) taskType, new TaskInfo(taskType, taskLength, taskConsoles));
            }

            return JsonSerializer.Serialize(tasks, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
                {
                    new Vector2Converter()
                }
            });
        }

        private string GetTaskLength(ShipStatus shipStatus, TaskTypes taskType)
        {
            if (shipStatus.CommonTasks.Any(commonTask => commonTask.TaskType == taskType))
            {
                return "common";
            }

            if (shipStatus.LongTasks.Any(longTask => longTask.TaskType == taskType))
            {
                return "long";
            }

            if (shipStatus.NormalTasks.Any(shortTask => shortTask.TaskType == taskType))
            {
                return "short";
            }

            return null;
        }

        public class TaskInfo
        {
            public string HudText { get; }
            public string Length { get; } // TODO length enum?
            public List<TaskConsole> Consoles { get; }

            public TaskInfo(TaskTypes taskType, string length, List<TaskConsole> consoles)
            {
                HudText = TranslationController.Instance.GetString(taskType);
                Length = length;
                Consoles = consoles;
            }
        }

        public class TaskConsole
        {
            public string HudText { get; }
            public SystemTypes Room { get; }
            public Vector2 Position { get; }
            public float UsableDistance { get; }

            public TaskConsole(TaskTypes taskType, SystemTypes room, Vector2 position, float usableDistance)
            {
                HudText = $"{TranslationController.Instance.GetString(room)}: {TranslationController.Instance.GetString(taskType)}";
                Room = room;
                Position = position;
                UsableDistance = usableDistance;
            }
        }

        public class Vector2Converter : JsonConverter<Vector2>
        {
            public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotSupportedException();
            }

            public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteNumber("x", value.x);
                writer.WriteNumber("y", value.y);
                writer.WriteEndObject();
            }
        }
    }
}