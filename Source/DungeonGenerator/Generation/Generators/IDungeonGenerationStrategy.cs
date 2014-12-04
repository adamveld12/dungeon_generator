using Dungeon.Generator.Navigation;

namespace Dungeon.Generator.Generation.Generators
{
    public interface IDungeonGenerationStrategy
    {
        void Execute();
    }


    public class MazeGeneratorStrategy : IDungeonGenerationStrategy
    {
        public void Execute()
        {
            /**

            drillers.push_back(make_pair(maze_size_x/2,maze_size_y/2)); 
            while(drillers.size()>0) 
            { 
               list < pair < int, int> >::iterator m,_m,temp; 
               m=drillers.begin(); 
               _m=drillers.end(); 
               while (m!=_m) 
               { 
                   bool remove_driller=false; 
                   switch(rand()%4) 
                   { 
                   case 0: 
                       (*m).second-=2; 
                       if ((*m).second<0 || maze[(*m).second][(*m).first]) 
                       { 
                           remove_driller=true; 
                           break; 
                       } 
                       maze[(*m).second+1][(*m).first]=true; 
                       break; 
                   case 1: 
                       (*m).second+=2; 
                       if ((*m).second>=maze_size_y || maze[(*m).second][(*m).first]) 
                       { 
                           remove_driller=true; 
                           break; 
                       } 
                       maze[(*m).second-1][(*m).first]=true; 
                       break; 
                   case 2: 
                       (*m).first-=2; 
                       if ((*m).first<0 || maze[(*m).second][(*m).first]) 
                       { 
                           remove_driller=true; 
                           break; 
                       } 
                       maze[(*m).second][(*m).first+1]=true; 
                       break; 
                   case 3: 
                       (*m).first+=2; 
                       if ((*m).first>=maze_size_x || maze[(*m).second][(*m).first]) 
                       { 
                           remove_driller=true; 
                           break; 
                       } 
                       maze[(*m).second][(*m).first-1]=true; 
                       break; 
                   } 
                   if (remove_driller) 
                       m = drillers.erase(m); 
                   else 
                   { 
                       drillers.push_back(make_pair((*m).first,(*m).second)); 
                       // uncomment the line below to make the maze easier 
                       // if (rand()%2) 
                       drillers.push_back(make_pair((*m).first,(*m).second)); 

                       maze[(*m).second][(*m).first]=true; 
                       ++m; 
                   } 
               } 
            } 
             **/
        }
    }
    
}