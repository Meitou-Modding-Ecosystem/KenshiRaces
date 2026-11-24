<a id="engine-references-refcheck"></a>
# Referências de motores (RefCheck)

Um mod é construído contra um motor (este repo `engine` Submode) e jogado em qualquer Meitou libertação o jogador tem. Quando uma versão muda a assinatura de algo que o mod chama, a fonte do mod ainda pode compilar, mas o DLL que já foi construído e que pede o jogo para o velho membro e falha no tempo com `MissingMethodException`.

Isso não é hipotético. Meitou 0.21 dá `Inventory.Add(ItemInstance)` O segundo parâmetro é opcional (`Add(ItemInstance, bool mainShut = false)`). Fonte que chama `bag.Add(item)` Compreendendo as versões, mas a DLL Construído contra 0.20 ou mais chama o método de um parâmetro, que 0.21 não tem mais. Em The Wasteland Remembers que rompeu os guardas em jogo, e a exceção parou um sistema para o resto da sessão. Nenhum teste o capturou: os testes compilam contra um motor.

<a id="what-it-checks"></a>
## O que verifica

`tools/RefCheck` Leia um Modo DLLOs metadados e listas de cada membro do motor que ele se refere (metódicos, construtores, campos em tipos de `Meitou*.dll`Por nome e parâmetros, que um dado não tem. Nada do modo ou do jogo funciona.

`tools/refcheck.sh` Construa todos os modos no repo (cada pasta de nível superior com `meitou.json` e `<Folder>/<Folder>.csproj`Descarregar uma versão win-x64 zip uma vez (só `Meitou*.dll` e `0Harmony.dll`e verifica cada assembléia do mod `meitou.json` As listas .

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

A saída, uma linha por lançamento e assembléia:

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

Ou, para uma pausa:

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## Em CI

O que `Engine references` Trabalho correndo `tools/refcheck.sh` contra a mais recente publicação em cada PR e pressão, e `Release readiness` Aguardando por isso. Um barco que embarca em um DLL O motor que se encontra em frente a um motor deve ser controlado DLL Para cada libertação `meitou.json` permitindo o

<a id="when-it-finds-something"></a>
## Quando encontramos algo

Chame o membro de uma maneira que funciona em cada lançamento que você suporta: reconstruir contra o motor mais novo, ou encontrar o membro por reflexão e preencher os parâmetros que você não passa com seus padrões. O deserto se lembra `InventoryCompat` O último para `Inventory.Add`.

As chamadas de reflexão para o motor não são verificadas: elas falham suavemente onde você os escreveu.
