using Code.TurnSystems;

namespace Code.Combat
{
    public static class CommandFactory
    {
        public static Command ConvertAttackTypeToCommand(int attackType,Team owner)
        {
            switch (attackType)
            {
                case 0:
                    return new LargeAttackCommand() {Owner = owner};
                case 1:
                    return new FlatAttackCommand() {Owner = owner};
                case 2:
                    return new RangeAttackCommand() {Owner = owner};
                default:
                    return new PassTurnCommand();
            }
        }
    }
}