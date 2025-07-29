using System;
using System.Globalization;
using System.Linq;
using System.Text;
using FishOverhaul.Content.Items;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using Terraria;
using Terraria.ModLoader;

namespace FishOverhaul;

public class Patches : ModSystem
{
    internal static string GetILString(ILCursor il) =>
        string.Join(
            '\n',
            il.Instrs.Select(i =>
                {
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    if (i == null)
                        return null;

                    var operand = i.Operand;
                    operand = operand switch
                    {
                        ILLabel label => label.Target,
                        ILLabel[] labels => labels.Select(l => l.Target).ToArray(),
                        _ => operand,
                    };

                    var sb = new StringBuilder($"{_getLabel(i)}: {i.OpCode.Name}");

                    if (operand == null)
                        return sb.ToString();

                    sb.Append(' ');

                    switch (i.OpCode.OperandType)
                    {
                        case OperandType.ShortInlineBrTarget:
                        case OperandType.InlineBrTarget:
                            sb.Append(_getLabel((Instruction)operand));
                            break;
                        case OperandType.InlineSwitch:
                            sb.Append(
                                string.Join(
                                    ',',
                                    ((Instruction[])operand).Select((label, n) => _getLabel(label))
                                )
                            );
                            break;
                        case OperandType.InlineString:
                            sb.Append($"\"{operand}\"");
                            break;
                        default:
                            sb.Append(Convert.ToString(operand, CultureInfo.InvariantCulture));
                            break;
                    }

                    return sb.ToString();
                })
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                .Where(i => i != null)
        );

    private static string _getLabel(Instruction i) => $"IL_{i.Offset:x4}";

    public override void OnModLoad()
    {
        var logger = ModContent.GetInstance<FishOverhaul>().Logger;

        IL_Projectile.AI_061_FishingBobber += il =>
        {
            var cursor = new ILCursor(il);
            logger.Debug($">> AI_061_FishingBobber cursor:{cursor}");
            logger.Debug($"IL:\n{GetILString(cursor)}");

            var addBuffMethod = typeof(Player).GetMethod(nameof(Player.AddBuff));
            var patchMethod = typeof(ShimmerBobber).GetMethod(nameof(ShimmerBobber._AddBuffPatch));

            if (
                addBuffMethod == null
                || patchMethod == null
                || !cursor.TryGotoNext(i =>
                    i.Operand is MethodReference func && func.Is(addBuffMethod)
                )
            )
            {
                logger.Warn(
                    $"Couldn't patch AddBuff call addBuffMethod:{(addBuffMethod == null ? "null" : addBuffMethod.ToString())} patchMethod:"
                );
                return;
            }

            cursor.Remove();
            cursor.EmitLdarg0();
            cursor.EmitCall(patchMethod);

            logger.Debug("Patched AddBuff call");
            logger.Debug($"IL:\n{GetILString(cursor)}");
        };
    }
}
