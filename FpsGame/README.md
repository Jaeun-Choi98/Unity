stateDiagram-v2
    Idle --> Move(attack)
    Move(attack) --> Attack
    Attack --> Move(attack)
    Attack --> AttackIdle
    AttackIdle --> Attack
    Move(attack) --> Return
    Return --> Idle
    AnyState --> Damaged
    Damaged --> Move(attack)
    AnyState --> Die
