
### 에너미 상태 전이도

```mermaid
stateDiagram-v2
    Idle --> Move(attack)
    
    Move(attack) --> Attack
    Attack --> Move(attack)
    Attack --> Attack_Idle
    Attack_Idle --> Attack
    Move(attack) --> Return
    Return --> Idle
   
    AnyState --> Damaged
    Damaged --> Move(attack)
    AnyState --> Die
    
    
