using System.Drawing;
using NUnit.Framework;

namespace Dungeon_Realms
{
    [TestFixture]
    class ModelTests
    {
        private GameObject[,] GetSimpleObjectMap(int size)
        {
            var result = new GameObject[size, size];
            for (var i = 0; i < size; i++)
                for (var j = 0; j < size; j++)
                    result[i, j] = new GameObject(result, new Point(i, j), false, null);
            return result;
        }

        private (GameObject[,] map, Hero hero) GetSimpleMap(int size, Point heroLocation)
        {
            var result = new GameObject[size, size];
            for (var i = 0; i < size; i++)
                for (var j = 0; j < size; j++)
                    result[i, j] = new Floor(result, new Point(i, j), null);
            var hero = new Hero(result, heroLocation, null);
            result[heroLocation.Y, heroLocation.X] = hero;
            return (result, hero);
        }

        [Test]
        public void GetIncident()
        {
            var map = GetSimpleObjectMap(3);
            var center = map[1, 1];
            Assert.AreEqual(map[0, 1], center.GetIncident(Direction.Up));
            Assert.AreEqual(map[2, 1], center.GetIncident(Direction.Down));
            Assert.AreEqual(map[1, 0], center.GetIncident(Direction.Left));
            Assert.AreEqual(map[1, 2], center.GetIncident(Direction.Right));
            Assert.IsNull(map[0, 0].GetIncident(Direction.Up));
            Assert.IsNull(map[0, 0].GetIncident(Direction.Left));
        }

        [Test]
        public void MoveHero()
        {
            (var map, var hero) = GetSimpleMap(2, new Point(0, 0));
            hero.TryMove(Direction.Right);
            Assert.AreEqual(new Point(0, 1), hero.Location);
            hero.TryMove(Direction.Up);
            Assert.AreEqual(new Point(0, 1), hero.Location);
        }

        [Test]
        public void MoveBox()
        {
            (var map, var hero) = GetSimpleMap(3, new Point(0, 0));
            var box = new Box(map, new Point(0, 1));
            map[0, 1] = box;
            hero.TryMove(Direction.Right);
            Assert.AreEqual(map[0, 1], hero);
            Assert.AreEqual(map[0, 2], box);
            hero.TryMove(Direction.Right);
            Assert.AreEqual(map[0, 1], hero);
            Assert.AreEqual(map[0, 2], box);
        }

        [Test]
        public void DontMoveTwoBox()
        {
            (var map, var hero) = GetSimpleMap(4, new Point(0, 0));
            var box1 = new Box(map, new Point(0, 1));
            var box2 = new Box(map, new Point(0, 2));
            map[0, 1] = box1;
            map[0, 2] = box2;
            hero.TryMove(Direction.Right);
            Assert.AreEqual(map[0, 0], hero);
            Assert.AreEqual(map[0, 1], box1);
            Assert.AreEqual(map[0, 2], box2);
        }

        [Test]
        public void MoveThroughHatch()
        {
            (var map, var hero) = GetSimpleMap(4, new Point(0, 0));
            var @in = new Hatch(map, new Point(0, 1));
            var @out = new Hatch(map, new Point(0, 2));
            @in.Out = @out;
            map[0, 1] = @in;
            map[0, 2] = @out;
            hero.TryMove(Direction.Right);
            Assert.AreEqual(map[0, 3], hero);
        }

        [Test]
        public void MoveThroughJoinedHatches()
        {
            (var map, var hero) = GetSimpleMap(6, new Point(0, 0));
            var in1 = new Hatch(map, new Point(0, 1));
            var out1 = new Hatch(map, new Point(0, 2));
            in1.Out = out1;
            map[0, 1] = in1;
            map[0, 2] = out1;
            var in2 = new Hatch(map, new Point(0, 3));
            var out2 = new Hatch(map, new Point(0, 4));
            in2.Out = out2;
            map[0, 3] = in2;
            map[0, 4] = out2;
            hero.TryMove(Direction.Right);
            Assert.AreEqual(map[0, 5], hero);
        }

        [Test]
        public void MoveBoxThroughHatch()
        {
            (var map, var hero) = GetSimpleMap(6, new Point(0, 0));
            var box = new Box(map, new Point(0, 1));
            map[0, 1] = box;
            var @in = new Hatch(map, new Point(0, 2));
            var @out = new Hatch(map, new Point(0, 3));
            @in.Out = @out;
            map[0, 2] = @in;
            map[0, 3] = @out;
            hero.TryMove(Direction.Right);
            Assert.AreEqual(map[0, 4], box);
            Assert.AreEqual(map[0, 1], hero);
            hero.TryMove(Direction.Right);
            Assert.AreEqual(map[0, 4], hero);
            Assert.AreEqual(map[0, 5], box);
        }

        [Test]
        public void DontMoveThroughHatch()
        {
            (var map, var hero) = GetSimpleMap(6, new Point(0, 0));
            var box1 = new Box(map, new Point(0, 3));
            map[0, 3] = box1;
            var box2 = new Box(map, new Point(0, 4));
            map[0, 4] = box2;
            var @in = new Hatch(map, new Point(0, 1));
            var @out = new Hatch(map, new Point(0, 2));
            @in.Out = @out;
            map[0, 1] = @in;
            map[0, 2] = @out;
            var eventOccurred = false;
            hero.CouldNotPassThroughHatch += hatch => eventOccurred = true;
            hero.TryMove(Direction.Right);
            Assert.IsTrue(eventOccurred);
            Assert.AreEqual(map[0, 0], hero);
        }

        [Test]
        public void ActivateAndDeactivatePressurePlate()
        {
            (var map, var hero) = GetSimpleMap(5, new Point(0, 0));
            var @in = new Hatch(map, new Point(0, 2));
            var @out = new Hatch(map, new Point(0, 4));
            @in.Out = @out;
            map[0, 2] = @in;
            map[0, 3] = @out;
            var plate = new PressurePlate(map, new Point(0, 1)) { Target = @in };
            @in.Close();
            map[0, 1] = plate;
            Assert.IsFalse(@in.IsOpened);
            Assert.IsFalse(@in.IsOpened);
            hero.TryMove(Direction.Right);
            Assert.IsTrue(@in.IsOpened);
            Assert.IsTrue(@in.IsOpened);
            hero.TryMove(Direction.Left);
            Assert.IsFalse(@in.IsOpened);
            Assert.IsFalse(@in.IsOpened);
        }

        [Test]
        public void ActivatePressurePlateWithBox()
        {
            (var map, var hero) = GetSimpleMap(5, new Point(0, 0));
            var box = new Box(map, new Point(0, 1));
            map[0, 1] = box;
            var @in = new Hatch(map, new Point(0, 2));
            var @out = new Hatch(map, new Point(0, 3));
            @in.Out = @out;
            @out.Out = @in;
            map[0, 3] = @in;
            map[0, 4] = @out;
            var plate = new PressurePlate(map, new Point(0, 2)) { Target = @in };
            @in.Close();
            map[0, 2] = plate;
            Assert.IsFalse(@in.IsOpened);
            Assert.IsFalse(@in.IsOpened);
            hero.TryMove(Direction.Right);
            Assert.IsTrue(@in.IsOpened);
            Assert.IsTrue(@in.IsOpened);
        }
    }
}
