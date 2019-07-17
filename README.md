# Índice
- [Índice](#%C3%8Dndice)
- [Configuração do ambiente](#Configura%C3%A7%C3%A3o-do-ambiente)
  - [Linux](#Linux)
    - [Instalação do Unity3D](#Instala%C3%A7%C3%A3o-do-Unity3D)
    - [Terminal](#Terminal)
    - [Instalação do VSCode (Linux)](#Instala%C3%A7%C3%A3o-do-VSCode-Linux)
  - [Windows](#Windows)
    - [Unity no Windows](#Unity-no-Windows)
    - [WSL (Windows Subsystem for Linux)](#WSL-Windows-Subsystem-for-Linux)
      - [Ativar Modo desenvolvedor](#Ativar-Modo-desenvolvedor)
      - [Terminal](#Terminal-1)
        - [Inicializar com bash como padrão](#Inicializar-com-bash-como-padr%C3%A3o)
      - [VSCode no WSL](#VSCode-no-WSL)
  - [Git com Unity (merge tool)](#Git-com-Unity-merge-tool)
- [Conteúdos](#Conte%C3%BAdos)
  - [Tutoriais](#Tutoriais)
    - [Básico](#B%C3%A1sico)
    - [Intermediário](#Intermedi%C3%A1rio)
    - [Avançado](#Avan%C3%A7ado)
  - [Textos/Artigos](#TextosArtigos)
  - [Livro](#Livro)
  - [prefabs](#prefabs)
  - [Vídeos](#V%C3%ADdeos)
- [""""Requisitos""""](#%22%22%22%22Requisitos%22%22%22%22)

# Configuração do ambiente
## Linux
### Instalação do Unity3D

```bash
$ wget "https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.AppImage" \
-O unityhub.AppImage

$ chmod +x unityhub.AppImage

$ ./unityhub.AppImage
```
### Terminal
```bash
sudo apt install terminator
```
ctrl+shift+e = splita terminal horizontalmente

ctrl+shift+o = splita terminal verticalmente
### Instalação do VSCode (Linux)

`$ sudo apt install code`


*Instalação de extensões úteis*
 
`$ bash` [vscode-extensions.sh](./../vscode-extensions.sh)


## Windows

### Unity no Windows
Download -> https://public-cdn.cloud.unity3d.com/hub/nuo/UnityHubSetup.exe?button=onboarding-download-btn-windows

### WSL (Windows Subsystem for Linux)
Download Ubuntu -> https://www.microsoft.com/pt-br/p/ubuntu-1804-lts/9n9tngvndl3q?activetab=pivot:overviewtab

#### Ativar Modo desenvolvedor
Windows + i -> "Atualização e Segurança" -> "Para Desenvolvedores" -> Seleciona "Modo Desenvolvedor"

Windows + x -> "PowerShell (Admin)" -> Cola o comando > 
Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux


#### Terminal
Download = https://releases.hyper.is/download/win

##### Inicializar com bash como padrão
Ctrl + , -> cola isso https://pastebin.com/1RQfUF2i


#### VSCode no WSL
tutorial = https://code.visualstudio.com/docs/remote/wsl

## Git com Unity (merge tool)
Ajuda a resolver os merges dos gameobjects etc
> [SmartMerge](https://docs.unity3d.com/Manual/SmartMerge.html)

# Conteúdos

## Tutoriais
https://unity3d.com/pt/learn/tutorials

### Básico
1) Interactive Tutorials
2) Roll-a-ball
3) Survival Shooter

### Intermediário
1) Roguelike
2) Tower Defense
3) Creating Beliavable Visuals

### Avançado
1) https://unity3d.com/pt/learn/tutorials/projects/adventure-game-tutorial

## Textos/Artigos
1) https://unity3d.com/pt/learn/tutorials/s/best-practices
2) https://docs.unity3d.com/Manual/AdvancedDevelopment.html

## Livro
http://gameprogrammingpatterns.com/contents.html

## prefabs
```
synty (packages na asset store mt picas, terrenos etc)
https://www.mixamo.com/#/ (gratuito)
```
## Vídeos
```markdown
**renderer sem codigo** - [link](https://www.youtube.com/watch?v=szsWx9IQVDI)
    continuar a ver o personagem (fantasminha) quando estiver passando atras de uma estrutura)
    obs: da pra usar esse projeto-exemplo como base pro battle royale
**terrenos** - [link](https://www.youtube.com/watch?v=MWQv2Bagwgk)
**A* / pathfinding** - [link](https://www.youtube.com/watch?v=jvtFUfJ6CP8)
**novidades 2019** - [link](https://www.youtube.com/watch?v=Vi7k7HtbTN0)
                   - [link](https://www.youtube.com/watch?v=PPAtFO2EMak)
    multiplayer dots
    unity visual searching - achar asset p jogo
    css pra mexer com UI?
    new input system

**principios de game design ( desenho )** - [link](https://www.youtube.com/watch?v=G8AT01tuyrk)
(achei bem "bom-senso" mas talvez vale a pena ver)
a diversão é o caminho entre o jogador e o objetivo

    3 pilares:
    jogador - comunicação - apelo

    # jogador
    qual é o papel do jogador?
    como ele interage com o jogo?
    o jogador sempre tem que ter um propósito
    o jogador que deve fazer o jogo progredir (isso que distingue um jogo de um filme)

    # comunicação
    tao importante quanto o jogador ter um propósito, é ele saber qual é o proposito dele
    deve manter o jogador focado no objetivo

    # apelo
    pode ser ação, gráficos estonteantes
    histórias interessantes
    puzzles dificeis etc

**multiplayer fps opensource da unity** - [link](https://www.youtube.com/watch?v=aTnBAzin9vE)
github: https://github.com/Unity-Technologies/FPSSample

**100 dicas de unity** - [link](https://www.youtube.com/watch?v=thA3zv0IoUM)
    dica 6 - script sem gameobject vazio
    dica 7 - copiar estado do objeto no playmode pro editmode
    dica 9 - as vezes usar struct em vez de classe (p evitar garbagecollector) [https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct]
    dica 16 - comparar distancias sem vector3.distance
    dica 17 - sempre usar text mesh pro
    dica 18-19-20 - [serializefield] p aparecer no editor, [hideininspector] p esconder, [formelyserializedas("x")] pra renomear
    dica 24 - ctrl shift f pra camera seguir a visao do editor
    dica 25 - metodo compareTag em vez de ==
    dica 28 - procurar nos assets -> t:Scripts, t:Terrains etc
    dica 33 - salvar layout do unity
    dica 42 - global c# define (project settings)
    dica 54 - plotar grafico p debugar valores d uma variavel ao longo do tempo
    dica 62 - string builder "em" + "vez" + "disso"
    dica 69 - ctrl + mover  mantem o objeto em posições exatas (?)
    dica xx - nunca usar camera.main
    dica 86 - iterar em todos os filhos
    dica 89 - "regioes" p poder colapsar pedaços de codigo (n sei se é so visual studio)
    dica 90 - pausar editor
```

# """"Requisitos""""
(só um bando de ideia solta do que deve ter no jogo, algumas são mais tecnicas outras abstratas)
```
scoreboard
    dano causado
    tempo da partida
    numero de kills

terreno
    gelo, agua, lava
    pantano (areia movediça)?

boneco (com armaduras e armas? inventario?)

fog of war
    moitas

team-up? 
    fazer timinho com um inimigo p tentar ganhar junto

comunicação 
    emojis, textos

pause
    opções

musica

sons de efeitos

animaçoes obviamente

minimapa

ai
    animais e monstros q atacam
    npcs que vendem coisas ? (igual no tibia)

particulas em explosoes etc
```