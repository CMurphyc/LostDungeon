using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//层数：
//1   (0 商店 2 宝箱房 1 事件 0 恶魔 1 boss ) tot: 4     map_size:5*7 16
//2   (1 商店 1 宝箱房 1 事件 1 恶魔 1 boss ) tot: 4     map_size:7*7 20
//3   (1 商店 1 宝箱房 2 事件 1 恶魔 1 boss ) tot: 5     map_size:8*8 24
//4   (2 商店 1 宝箱房 2 事件 1 恶魔 1 boss ) tot: 6     map_size:9*9 28
//5   (2 商店 1 宝箱房 3 事件 1 恶魔 1 boss ) tot: 7     map_size:10*10 32
//0没有东西 1:普通房间 2：L房 3:长直道 4:大房
//5:boss房 6:恶魔房 7: 商店 8:挑战房 9：游戏房 10:献祭房 11：宝箱房
public class RandMap
{
    public static HashSet<Tuple<int, int>>[] s = new HashSet<Tuple<int, int>>[5];
    public static int[][] type = new int[20][];
    public static int[][] d = new int[20][];
    public static int[][] nex = new int[4][];
    public static int width, height, len;
    public static int pNum, seed, depth;
    public static int n;
    public static bool ret = false;
    public static void init()
    {
        for (int i = 0; i < 20; i++) type[i] = new int[20];
        for (int i = 0; i < 20; i++) d[i] = new int[20];
        s = new HashSet<Tuple<int, int>>[5];//不知道能不能重新初始化
        for (int i = 0; i < 5; i++)
            s[i] = new HashSet<Tuple<int, int>>();
        nex[0] =new int[] {1,0};
        nex[1] =new int[] {-1,0};
        nex[2] =new int[] {0,1};
        nex[3] =new int[] {0,-1};

        if (depth == 1)
        {
            n = 4;
            width = 5;
            height = 7;
            len = 1;
        }
        else if (depth == 2)
        {
            n = 4;
            width = 7;
            height = 7;
            len = 1;
        }
        else if (depth == 3)
        {
            n = 5;
            width = 8;
            height = 8;
            len = 2;
        }
        else if (depth == 4)
        {
            n = 6;
            width = 9;
            height = 9;
            len = 2;
        }
        else
        {       
            n = 7;
            width = 10;
            height = 10;
            len = 2;
        }
    }

    public static void AddFrame()//将边框分到set里面
    {
        for (int i = len + 1; i <= width - len; i++)//up
        {
            for (int j = 1; j <= len; j++)
            {
                s[0].Add(new Tuple<int, int>(j, i));
                s[1].Add(new Tuple<int, int>(j, i));
            }
        }

        for(int i=len+1;i<=width-len;i++)//down
        {
            for(int j=0;j<len;j++)
            {
                s[0].Add(new Tuple<int,int>(height - j, i));
                s[2].Add(new Tuple<int,int>(height - j, i));
            }
        }

        for(int i=len+1;i<=height-len;i++)//left
        {
            for(int j=1;j<=len;j++)
            {
                s[0].Add(new Tuple<int,int>(i,j));
                s[3].Add(new Tuple<int,int>(i,j));
            }
        }

        for(int i=len+1;i<=height-len;i++)//right
        {
            for(int j=0;j<len;j++)
            {
                s[0].Add(new Tuple<int,int>(i, width - j));
                s[4].Add(new Tuple<int,int>(i, width - j));
            }
        }
    }

    public static void ERASE(Tuple<int,int> ret, int w)//删除某个点以及周围的点
    {
        for (int i = -w; i <= w; i++)
        {
            for (int j = -w; j <= w; j++)
            {
                Tuple<int, int> p = new Tuple<int, int>( ret.Item1+i,ret.Item2+j );
                for (int k = 0; k < 5; k++)
                {
                    if (s[k].Contains(p)) s[k].Remove(p);
                }
            }
        }
    }

    public static void RAND(int k)//随机点
    {
        int x = seed % s[k].Count;
        foreach(Tuple<int,int> p in s[k])
        {
            if(x==0)
            {
                Tuple<int, int> ret = p;
                type[ret.Item1][ret.Item2] = 11;
                ERASE(ret, 1);
                break;
            }
            x--;
        }
    }

    public static bool judge(Tuple<int,int> p, int t)
    {
        if (t == 1)
        {
            if (type[p.Item1 + 1][p.Item2] == 1 && type[p.Item1 + 1][p.Item2 + 1] == 1 && type[p.Item1][p.Item2 + 1] == 1 &&
               type[p.Item1 - 1][p.Item2] == 0 && type[p.Item1 - 1][p.Item2 - 1] == 0 && type[p.Item1][p.Item2 - 1] == 0
               )
            {
                type[p.Item1][p.Item2] = 4;
                type[p.Item1 + 1][p.Item2] = type[p.Item1 + 1][p.Item2 + 1] = type[p.Item1][p.Item2 + 1] = 4; return true;
            }

        }
        else if (t == 2)
        {
            if (type[p.Item1 + 1][p.Item2] == 1 && type[p.Item1 + 1][p.Item2 - 1] == 1 && type[p.Item1][p.Item2 - 1] == 1 &&
               type[p.Item1 - 1][p.Item2] == 0 && type[p.Item1 - 1][p.Item2 + 1] == 0 && type[p.Item1][p.Item2 + 1] == 0
               )
            {
                type[p.Item1][p.Item2] = 4;
                type[p.Item1 + 1][p.Item2] = type[p.Item1 + 1][p.Item2 - 1] = type[p.Item1][p.Item2 - 1] = 4; return true;
            }

        }
        else if (t == 3)
        {
            if (type[p.Item1 - 1][p.Item2] == 1 && type[p.Item1 - 1][p.Item2 - 1] == 1 && type[p.Item1][p.Item2 - 1] == 1 &&
               type[p.Item1 + 1][p.Item2] == 0 && type[p.Item1 + 1][p.Item2 + 1] == 0 && type[p.Item1][p.Item2 + 1] == 0
               )
            {
                type[p.Item1][p.Item2] = 4;
                type[p.Item1 - 1][p.Item2] = type[p.Item1 - 1][p.Item2 - 1] = type[p.Item1][p.Item2 - 1] = 4; return true;
            }

        }
        else
        {
            if (type[p.Item1 - 1][p.Item2] == 1 && type[p.Item1 - 1][p.Item2 + 1] == 1 && type[p.Item1][p.Item2 + 1] == 1 &&
               type[p.Item1 + 1][p.Item2] == 0 && type[p.Item1 + 1][p.Item2 - 1] == 0 && type[p.Item1][p.Item2 - 1] == 0
               )
            {
                type[p.Item1][p.Item2] = 4;
                type[p.Item1 - 1][p.Item2] = type[p.Item1 - 1][p.Item2 + 1] = type[p.Item1][p.Item2 + 1] = 4; return true;
            }

        }
        return false;
    }

    static public bool judgeL(Tuple<int, int> p, int t)
    {
        if (t == 1)
        {
            if (type[p.Item1 + 1][p.Item2] == 1 && type[p.Item1 + 1][p.Item2 + 1] == 1 && type[p.Item1][p.Item2 + 1] == 1
               )
            {
                type[p.Item1 + 1][p.Item2] = type[p.Item1 + 1][p.Item2 + 1] = type[p.Item1][p.Item2 + 1] = 2; return true;
            }

        }
        else if (t == 2)
        {
            if (type[p.Item1 + 1][p.Item2] == 1 && type[p.Item1 + 1][p.Item2 - 1] == 1 && type[p.Item1][p.Item2 - 1] == 1
               )
            {
                type[p.Item1 + 1][p.Item2] = type[p.Item1 + 1][p.Item2 - 1] = type[p.Item1][p.Item2 - 1] = 2; return true;
            }

        }
        else if (t == 3)
        {
            if (type[p.Item1 - 1][p.Item2] == 1 && type[p.Item1 - 1][p.Item2 - 1] == 1 && type[p.Item1][p.Item2 - 1] == 1
               )
            {
                type[p.Item1 - 1][p.Item2] = type[p.Item1 - 1][p.Item2 - 1] = type[p.Item1][p.Item2 - 1] = 2; return true;
            }

        }
        else
        {
            if (type[p.Item1 - 1][p.Item2] == 1 && type[p.Item1 - 1][p.Item2 + 1] == 1 && type[p.Item1][p.Item2 + 1] == 1
               )
            {
                type[p.Item1 - 1][p.Item2] = type[p.Item1 - 1][p.Item2 + 1] = type[p.Item1][p.Item2 + 1] = 2; return true;
            }

        }
        return false;
    }

    static public bool judgeZ(Tuple<int, int> p, int t)
    {
        if (t == 1)
        {
            if (type[p.Item1 + 1][p.Item2] == 1
               )
            {
                type[p.Item1 + 1][p.Item2] = type[p.Item1][p.Item2] = 3; return true;
            }

        }
        else if (t == 2)
        {
            if (type[p.Item1][p.Item2 + 1] == 1
               )
            {
                type[p.Item1][p.Item2 + 1] = type[p.Item1][p.Item2] = 3; return true;
            }

        }
        else if (t == 3)
        {
            if (type[p.Item1 - 1][p.Item2] == 1
               )
            {
                type[p.Item1 - 1][p.Item2] = type[p.Item1][p.Item2] = 3; return true;
            }

        }
        else
        {
            if (type[p.Item1][p.Item2 - 1] == 1
               )
            {
                type[p.Item1][p.Item2 - 1] = type[p.Item1][p.Item2] = 3; return true;
            }

        }
        return false;
    }
    static public void Rander()
    {
        init();
        AddFrame();
        n -= 4;
        for (int i = 1; i <= 4; i++) RAND(i);
        for(int i=0;i<n;i++)
        {
            RAND(0);
        }

        if (len == 2)
        {
            for (int i = 1; i <= height; i++)
                for (int j = 1; j <= width; j++)
                    if (type[i][j] == 11)
                    {
                        if (i == 1)
                        {
                            type[i + 1][j] = 1;
                        }
                        else if (i == height)
                        {
                            type[i - 1][j] = 1;
                        }
                        else if (j == 1)
                        {
                            type[i][j + 1] = 1;
                        }
                        else if (j == width)
                        {
                            type[i][j - 1] = 1;
                        }
                    }
        }
        Tuple<int, int> bz = new Tuple<int, int>(1, 1);//
        for (int i = len + 1; i <= width - len; i++)
        {
            if (type[len][i] != 0)
            {
                bz =new Tuple<int, int>(len,i);
                break;
            }
        }
        int maxn = bz.Item1 + 1;
        for (int i = len + 1; i <= height - len; i++)
        {
            for (int j = 1; j <= width; j++)
            {
                if (type[i][j] != 0)
                    maxn =Mathf.Max(maxn, i);
            }
        }
        for (int i = bz.Item1 + 1; i <= maxn; i++)
            type[i][bz.Item2] = 1;


        for (int i = len + 1; i <= height - len; i++)
        {
            if (type[i][len] != 0)
            {
                for (int j = len + 1; ; j++)
                {
                    if (type[i][j] != 0) break;
                    type[i][j] = 1;
                }
            }
        }

        for (int i = len + 1; i <= height - len; i++)
        {
            if (type[i][width - len + 1] != 0)
            {
                for (int j = width - len; ; j--)
                {
                    if (type[i][j] != 0) break;
                    type[i][j] = 1;
                }
            }
        }

        for (int i = len + 1; i <= width - len; i++)
        {
            if (type[len][i] != 0)
            {
                for (int j = len + 1; ; j++)
                {
                    if (type[j][i] != 0) break;
                    type[j][i] = 1;
                }
            }
        }

        for (int i = len + 1; i <= width - len; i++)
        {
            if (type[height - len + 1][i] != 0)
            {
                for (int j = height - len; ; j--)
                {
                    if (type[j][i] != 0) break;
                    type[j][i] = 1;
                }
            }
        }

        int cnt = 1;
        for (int i = 1; i <= height; i++)
        {
            for (int j = 1; j <= width; j++)
            {
                if (type[i][j] == 0 && ((seed / (i + j)) % 2 == 0 || cnt == 1))
                {
                    for (int k = 1; k <= 4; k++)
                    {
                        if (judge(new Tuple<int, int>(i,j),k))
                        {
                            cnt--;
                            break;
                        }
                    }
                }
            }
        }

        cnt=1;
        for(int i=1;i<=height;i++)
        {
            for(int j=1;j<=width;j++)
            {
                if(type[i][j]==0&&((seed/i)%2==0||(seed/j)%2==0||cnt==1))
                {
                    for(int k=1;k<=4;k++)
                    {
                        if(judgeL(new Tuple<int,int>(i,j),k))
                        {
                            cnt--;
                            break;
                        }
                    }
                }
            }
        }

        
        HashSet<Tuple<int, int>> ts = new HashSet<Tuple<int, int>>();
        for(int i=1;i<=height;i++)
        {
            for(int j=1;j<=width;j++)
                if(type[i][j]==11)
                    ts.Add(new Tuple<int,int> (i,j));
        }
        int bossnum = seed % (ts.Count);
        Tuple<int, int> bosspos=new Tuple<int, int>(0,0);////////////////
        foreach(Tuple<int,int> it in ts)
        {
            if(bossnum==0)
            {
                bosspos = it;
                type[bosspos.Item1][bosspos.Item2] = 5;
                if(ts.Contains(bosspos))
                ts.Remove(bosspos);
                break;
            }
            bossnum--;
        }
        //恶魔房
        for(int i=0;i<4;i++)
        {
            int tx = bosspos.Item1 + nex[i][0];
            int ty = bosspos.Item2 + nex[i][1];
            if(type[tx][ty]!=0) continue;
            bool can = true;
            for(int j=-1;j<=1;j++)
            {
                for(int k=-1;k<=1;k++)
                {
                    int tj = tx + j;
                    int tk = ty + k;
                    if(tj==0&&tk==0) continue;
                    if(tj==bosspos.Item1&&tk==bosspos.Item2) continue;
                    if (tj < 0 || tk <0) continue;
                    if(type[tj][tk]!=0) can=false;
                }
            }
            if(can)
            {
                type[tx][ty]=6;
                break;
            }
        }
        //born
        Queue<Tuple<int, int>> qu=new Queue<Tuple<int, int>>();
        qu.Enqueue(bosspos);
        d[bosspos.Item1][bosspos.Item2]=1;
        while(qu.Count!=0)
        {
            Tuple<int, int> x = qu.Dequeue();
            for(int i=0;i<4;i++)
            {
                int tx = x.Item1 + nex[i][0];
                int ty = x.Item2 + nex[i][1];
                if (tx < 0 || ty < 0) continue;
                if(type[tx][ty]==0||d[tx][ty]!=0) continue;
                d[tx][ty]=d[x.Item1][x.Item2]+1;
                qu.Enqueue(new Tuple<int,int>(tx,ty));
            }
        }
        int maxdist = 0;
        for(int i=1;i<=height;i++)
        {
            for(int j=1;j<=width;j++)
            {
                if(d[i][j]!=0&&type[i][j]==1)
                {
                    maxdist=Mathf.Max(maxdist, d[i][j]);
                }
            }
        }
        maxdist=maxdist/2+seed%depth+1;
        Tuple<int, int> bornpos = new Tuple<int, int>(-1, -1);
        for(int i=1;i<=height;i++)
        {
            for(int j=1;j<=width;j++)
            {
                if(d[i][j]!=0&&type[i][j]==1)
                {
                    if(bornpos.Item1==-1)
                    {
                        bornpos=new Tuple<int,int>(i,j);
                    }
                    else
                    {
                        int dis1 = Mathf.Abs(d[bornpos.Item1][bornpos.Item2] - maxdist), dis2 = Mathf.Abs(d[i][j] - maxdist);
                        if(dis1>dis2)
                        {
                            bornpos= new Tuple<int, int>(i,j);
                        }
                    }
                }
            }
        }
        type[bornpos.Item1][bornpos.Item2]=12;

        //直道

        for(int i=1;i<=height;i++)
        {
            for(int j=1;j<=width;j++)
            {
                if(type[i][j]==1&&((seed/i)%2==0||(seed/j)%2==0))
                {
                    for(int k=1;k<=4;k++)
                    {
                        if(judgeZ(new Tuple<int,int>(i,j),k))
                        {
                            i+=3;
                            j=1;
                            break;
                        }
                    }
                }
            }
        }
        
    //结束时可以来个随机旋转
    }

    static public void StartRand(int Seed,int PNUM,int Depth)
    {
        seed = Seed;
        pNum = PNUM;
        depth = Depth;
        Rander();
    }

    static public int GetWidth()
    {
        return width+1;
    }

    static public int GetHeight()
    {
        return height+1;
    }

    static public int GetValue(int i,int j)
    {
        return type[i][j];
    }

}
