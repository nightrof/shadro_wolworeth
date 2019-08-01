# Referências

1.  [Expressed Unity, FPS Multiplayer Game](https://www.youtube.com/watch?v=MeFElBan4u4)

1. [Info Gamer, Photon 2 Setup](https://www.youtube.com/watch?v=02P_mrszvzY)
   
2. [Info Gamer, How to make a Multiplayer Game](https://www.youtube.com/watch?v=phDySdEKXcw&list=PLWeGoBm1YHVgXmitft-0jkvcTVhAtL9vG)


(mais utilizada até o momento = Info Gamer)


# Primeiro Passo - Prototipo de jogo

## Conceito

Um "jogo" estilo [roll-a-ball](https://unity3d.com/pt/learn/tutorials/s/roll-ball-tutorial), onde há X bolas num mapa (?), e só há o controle direcional.

Cada bola tem Y de vida, e possuem uma aceleração grande

Ao se chocarem, sofrem/dão W de dano (baseado na velocidade)

A última bola viva, vence.


## Passo-a-passo

### Terreno/Arena

#### Packages Utilizados
* [Terrain Tools Sample Asset Pack](https://assetstore.unity.com/packages/2d/textures-materials/terrain-tools-sample-asset-pack-145808)

* [World Space Trees (FREE)](https://assetstore.unity.com/packages/vfx/shaders/world-space-trees-free-shader-117088)

#### Referência
* [How to make Terrain in Unity!](https://www.youtube.com/watch?v=MWQv2Bagwgk)


#### Resultado
![close](/prototipo/close-look.png)
![upper](/prototipo/upper-look.png)

### Bola/Jogador

#### Packages Utilizados

#### Referência
* [Roll-a-Ball](https://unity3d.com/pt/learn/tutorials/s/roll-ball-tutorial)

#### Descrição

Há 3 scripts básicos:
* [PlayerController](https://github.com/nightrof/shadro_wolworeth/blob/master/prototipo/Multiplayer%20Roll-a-Ball/Assets/Scripts/PlayerController.cs)
    * ```
      Além de controlar os inputs (setas e WASD) para movimentar a bola,
      calcula a velocidade da mesma em Km/h
      ```
* [CameraController](https://github.com/nightrof/shadro_wolworeth/blob/master/prototipo/Multiplayer%20Roll-a-Ball/Assets/Scripts/CameraController.cs)
    * ```
      Garante que a câmera siga a bola
      ```

* [CollisionHandler](https://github.com/nightrof/shadro_wolworeth/blob/master/prototipo/Multiplayer%20Roll-a-Ball/Assets/Scripts/CollisionHandler.cs)
    * ```
      Trata as colisões, ao colidir com outro Player, recebe X de dano
      (de acordo com a velocidade do objeto que veio a colidir)

      Ao chegar a 0 de vida, é destruído
      ```

#### Resultado
![bola](/prototipo/balls.png)

### Multiplayer

#### Packages Utilizados
* [PUN 2 - FREE](https://assetstore.unity.com/packages/tools/network/pun-2-free-119922)
* TextMeshPro (já incluso no Unity3d)

#### Referência
* [Unity Multiplayer Tutorial - Photon 2 Setup (Part 1)](https://www.youtube.com/watch?v=02P_mrszvzY)
  * ###### Obs: aos 4:30 o Wizard do Pun abre automaticamente; Caso isso não aconteça, alt+p (ou Window -> Photon Unity Networking -> PUN Wizard) deve abrir (caso nao abra, reinicie o Unity)
* [Unity Multiplayer Tutorial - Quick Start Matchmaking (Part 2)](https://www.youtube.com/watch?v=SNhWbHqFUbU)

#### Descrição

Todos os códigos relacionados à multiplayer/photon/network, foram tirados do [canal InfoGamer](https://www.youtube.com/channel/UCyoayn_uVt2I55ZCUuBVRcQ).

Links para os dois artigos:
- [Multiplayer Setup](http://www.infogamerhub.com/multiplayer-setup/)
  * Conectar ao Photon
- [Menu/Quickstart](http://www.infogamerhub.com/multiplayer-quick-start/)
  * Entrar e/ou criar salas
#### Resultado

![botao start](/prototipo/Menu_Start.png)

![botao cancelar](/prototipo/Menu_Cancel.png)

![dentro do jogo](/prototipo/Inside_Game.png)

https://www.youtube.com/watch?v=ayiXNHhUhQE
https://www.youtube.com/watch?v=VCWIl8WWJF0
https://www.youtube.com/watch?v=DEtZUeVY9qk
https://www.youtube.com/watch?v=xenW67bXTgM
https://gamedev.stackexchange.com/questions/144956/jump-script-component-for-character-controller
https://answers.unity.com/questions/373508/footsteps-script-for-running-and-walking.html