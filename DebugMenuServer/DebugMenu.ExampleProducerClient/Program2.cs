// // See https://aka.ms/new-console-template for more information
//
// using DebugMenu.ProducerClientSdk;
//
// Console.WriteLine("Hello, World!");
//
// var producer = new Producer("https://localhost:8082", "hej");
// var id = "8c57f858-7d30-4b34-b32c-df88e677a267";
//
// //var success = await producer.Start(new Dictionary<string, string>());
// var success = await producer.Rejoin($"wss://localhost:8082/ws/room/{id}", id);
//
// Console.WriteLine(success);
//
// var controller = new Controller($"wss://localhost:8082/ws/room/{producer.SessionId}/controller", "hej");
//
// await controller.Start(new Dictionary<string, string>());
//
// var t = Task.WhenAny(
//     Task.Run(async () => {
//         while (true) {
//             await producer.Receive();
//         }
//     }),
//     Task.Run(async () => {
//         while (true) {
//             await controller.Receive();
//         }
//     }));
//
// while (!t.IsCompleted) {
//     string? line = Console.ReadLine();
//
//     producer.Send(line!);
// }