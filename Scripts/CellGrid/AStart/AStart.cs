using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Units;
using System.Collections.Generic;
using System.Linq;

namespace SrpgFramework.CellGrid.AStar
{
    public class AStart
    {
        public static List<Cell> GetPath(Cell start, Cell end, MoveUnit unit)
        {
            Dictionary<Cell, AStartNode> nodeDict = new Dictionary<Cell, AStartNode>() { { start, new AStartNode(0, start.GetDistance(end), null) } };   //��¼ÿ�����ӵ�Ѱ·��Ϣ
            var open = new HashSet<Cell>() { start };  //��ѡ�б�
            var close = new HashSet<Cell>();    //�Ѵ����б�

            while (open.Any())
            {
                var current = open.First();
                foreach (var node in open)
                {
                    if (nodeDict[node].F < nodeDict[current].F || nodeDict[node].F == nodeDict[current].F && nodeDict[node].H < nodeDict[current].H)
                        current = node;     //�ҵ���ѽڵ�
                }

                if (current == end) //����ýڵ�ΪĿ��� ѭ������ ����Path
                {
                    var path = new List<Cell>();
                    var currentPathTile = current;
                    while (currentPathTile != start)    //·��������������
                    {
                        path.Insert(0, currentPathTile);
                        currentPathTile = nodeDict[currentPathTile].Connection;
                    }
                    return path;
                }

                close.Add(current);     //���ýڵ���Ϊ�Ѵ���
                open.Remove(current);   //���Ƴ��������б�

                foreach (var neighbor in current.Neighbors)     //�����ھӽڵ�
                {
                    if (close.Contains(neighbor) || !unit.IsCellTraversable(neighbor))   //�����Ѵ�����޷�ͨ�еĽڵ�
                        continue;

                    var costToNeighbor = nodeDict[current].G + neighbor.MoveCost;   //����G = ��ǰ�ڵ�G + MoveCost

                    if (!open.Contains(neighbor))    //����ڵ�û�����������ѡ�б� ������һ��node
                    {
                        open.Add(neighbor);
                        nodeDict.Add(neighbor, new AStartNode(costToNeighbor, neighbor.GetDistance(end), current));
                    }
                    else if (costToNeighbor < nodeDict[neighbor].G) //�����ǰ�ڵ��ں�ѡ�б���&&�ӵ�ǰ�ڵ㵽�ýڵ��Gֵ��С ���½ڵ�
                    {
                        nodeDict[neighbor].SetG(costToNeighbor);
                        nodeDict[neighbor].SetConnection(current);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// �ƶ���Χ����ˮ�㷨
        /// </summary>
        public static HashSet<Cell> GetMoveableArea(Cell start, MoveUnit unit, int step)
        {
            HashSet<Cell> open = new(); //������
            HashSet<Cell> close = new();    //�Ѵ���
            HashSet<Cell> nextOpen = new(); //��һ�ֵĴ�����
            HashSet<Cell> result = new();   //���ؽ��

            open.Add(start);
            close.Add(start);

            for (var i = 1; i <= step; i++)
            {
                foreach (var cell in open)
                {
                    foreach (var neighbor in cell.Neighbors)
                    {
                        if (close.Contains(neighbor))
                            continue;               //�Ѵ����Cell������
                        else
                            close.Add(neighbor);    //��Cell���Ϊ�Ѵ���

                        if (i < step && unit.IsCellTraversable(neighbor))
                        {
                            result.Add(neighbor);
                            nextOpen.Add(neighbor);
                        }
                        //���һ����Ҫ���Ǹ����Ƿ�ռ��
                        else if (i == step && unit.IsCellMovableTo(neighbor))
                        {
                            result.Add(neighbor);
                        }
                    }
                }

                open.Clear();
                open.UnionWith(nextOpen);
                nextOpen.Clear();
            }
            
            return result;
        }
    }

    internal struct AStartNode
    {
        /// <summary>
        /// ����㵽�õ��ʵ�ʾ���
        /// </summary>
        public int G { get; set; }
        /// <summary>
        /// �Ӹõ㵽�յ�Ĺ�����루�ֹۣ�
        /// </summary>
        public int H { get; set; }
        /// <summary>
        /// �ܹ�ֵ
        /// </summary>
        public int F => G + H;
        /// <summary>
        /// ��һ���ڵ� ��־�Ǵ��ĸ��ڵ㵽��ýڵ��
        /// </summary>
        public Cell Connection { set; get; }

        public AStartNode(int g, int h, Cell connection)
        {
            G = g;
            H = h;
            Connection = connection;
        }

        public void SetG(int g)
        {
            G = g;
        }
        public void SetH(int h)
        {
            H = h;
        }
        public void SetConnection(Cell connection)
        {
            this.Connection = connection;
        }
    }
}