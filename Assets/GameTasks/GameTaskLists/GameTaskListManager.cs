using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameTasks.GameTaskLists
{
    public class GameTaskListManager
    {
        public GameTaskList PlayerGameTaskList = new GameTaskList();
        public GameTaskList BulletGameTaskList = new GameTaskList();
        public GameTaskList EnemyGameTaskList = new GameTaskList();
        public GameTaskList EnemyBulletGameTaskList = new GameTaskList();
        public GameTaskList UIElementGameTaskList = new GameTaskList();

        public void RunFrames(float deltaTime)
        {
            PlayerGameTaskList.RunFrames(deltaTime);
            BulletGameTaskList.RunFrames(deltaTime);
            EnemyGameTaskList.RunFrames(deltaTime);
            EnemyBulletGameTaskList.RunFrames(deltaTime);
            UIElementGameTaskList.RunFrames(deltaTime);
        }

        public void AddPlayerTask(GameTask task)
        {
            PlayerGameTaskList.Add(task);
        }
        public void AddBulletTask(GameTask task)
        {
            BulletGameTaskList.Add(task);
        }
        public void AddEnemyTask(GameTask task)
        {
            EnemyGameTaskList.Add(task);
        }
        public void AddEnemyBulletTask(GameTask task)
        {
            EnemyBulletGameTaskList.Add(task);
        }
        public void AddUIElementTask(GameTask task)
        {
            UIElementGameTaskList.Add(task);
        }
    }
}
