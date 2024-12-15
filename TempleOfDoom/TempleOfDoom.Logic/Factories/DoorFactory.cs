//using TempleOfDoom.Data;
//using TempleOfDoom.Logic.Decorators;

//namespace TempleOfDoom.Logic.Factories;

//public class DoorFactory
//{
//    public IDoor createDoor(ConnectionDto connection, Room firstRoom, Room secondRoom, Player player)
//    {
//        for (int i = 0; i < connection.doors.Length; i++)
//        {
//            var door = connection.

//            switch (door.type)
//            {
//                case "default":
//                    IDoor defaultDoor = new DefaultDoor();
//                    defaultDoor.addRoom(d, firstRoom);

//                case "colored":
//                    return new ColoredDecorator(new DefaultDoor(doorDto.), doorDto.color, player);
//                case "toggle":
//                    return new ToggleDecorator(new DefaultDoor());
//                case "closing gate":
//                    return new ClosingDecorator(new DefaultDoor());
//                case "open on odd":
//                    return new OpenOnOddDecorator(new DefaultDoor(), player);
//                case "open on stones in room":
//                    return new OpenOnStonesInRoomDecorator(new DefaultDoor(), doorDto.no_of_stones, player);
//            }
//        }
//    }
//}