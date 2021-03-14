using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Util;

namespace Assets.GameTasks.GameTaskLists
{
    public class GameTaskListManager
    {
        public GameTaskList PlayerGameTaskList = new GameTaskList();
        public GameTaskList BulletGameTaskList = new GameTaskList();
        public GameTaskList EnemyGameTaskList = new GameTaskList();
        public GameTaskList EnemyBulletGameTaskList = new GameTaskList();
        public GameTaskList UIElementGameTaskList = new GameTaskList();

        public void RunFrames(float playerDt, float bulletDt, float enemyDt, float uiDt)
        {
            PlayerGameTaskList.RunFrames(playerDt);
            BulletGameTaskList.RunFrames(bulletDt);
            EnemyGameTaskList.RunFrames(enemyDt);
            EnemyBulletGameTaskList.RunFrames(enemyDt);
            UIElementGameTaskList.RunFrames(uiDt);
        }

        private GameTaskList GetList(GameTaskType taskType)
        {
            GameTaskList ret;
            switch (taskType)
            {
                case GameTaskType.Player:
                    ret = PlayerGameTaskList;
                    break;
                case GameTaskType.PlayerBullet:
                    ret = BulletGameTaskList;
                    break;
                case GameTaskType.Enemy:
                    ret = EnemyGameTaskList;
                    break;
                case GameTaskType.EnemyBullet:
                    ret = EnemyBulletGameTaskList;
                    break;
                case GameTaskType.UIElement:
                    ret = UIElementGameTaskList;
                    break;
                default:
                    throw ExceptionUtil.ArgumentException(() => taskType);
            }
            return ret;
        }


        public void StartTask(GameTask task, GameTaskType taskType)
        {
            var list = GetList(taskType);
            list.Add(task);
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

        public void GameTaskRunnerDeactivated(ValkyrieSprite target)
        {
            var taskType = target.TaskType;
            var list = GetList(taskType);

            list.RemoveTasksRelatedToTarget(target);
        }


        public void SetDebugUi()
        {
            DebugUI.SetDebugLabel("PlayerGameTaskList", () => PlayerGameTaskList.Count);
            DebugUI.SetDebugLabel("BulletGameTaskList", () => BulletGameTaskList.Count);
            DebugUI.SetDebugLabel("EnemyGameTaskList", () => EnemyGameTaskList.Count);
            DebugUI.SetDebugLabel("EnemyBulletGameTaskList", () => EnemyBulletGameTaskList.Count);
            DebugUI.SetDebugLabel("UIElementGameTaskList", () => UIElementGameTaskList.Count);
        }
    }
}
