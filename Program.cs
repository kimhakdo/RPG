using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

namespace kimhakdo
{
    internal class Program                                                            // 스스로 해보려하다 결국..GPT의 도움을 받았습니다..
    {
        static void Main(string[] args)
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");

            Console.Write("당신의 이름이 어떻게 되십니까? ");
            string name = Console.ReadLine();

            Console.WriteLine("=============================================================");
            Console.WriteLine("환영합니다. " + name + "님!");

            string job = "";
            string behavior = "";

            int gold = 1500;
            int attack = 10;
            int defence = 5;
            int HP = 100;

            float Level = 1;

            List<Item> inventory = new List<Item>();

            while (true)
            {
                Console.WriteLine("직업을 선택해 주세요");
                Console.WriteLine("1. 전사 / 2. 마법사 / 3. 궁수");

                switch (Console.ReadLine())
                {
                    case "1":
                        job = "전사";
                        break;
                    case "2":
                        job = "마법사";
                        break;
                    case "3":
                        job = "궁수";
                        break;

                    default:
                        Console.WriteLine("                            ");
                        Console.WriteLine("다시 선택해 주시기 바랍니다.");
                        continue;
                }
                Console.WriteLine($"{job}를 선택하셨습니다.");
                break;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine("1. 상태보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("0. 게임 종료");

                Console.WriteLine("원하시는 행동을 입력해주세요.");
                behavior = Console.ReadLine();

                switch (behavior)
                {
                    case "1":
                        ShowCharacterStatus(Level, name, job, gold, attack, defence, HP);
                        break;

                    case "2":
                        ShowInventory(ref attack, ref defence, inventory);
                        break;

                    case "3":
                        Shop(ref gold, inventory);
                        break;

                    case "0":
                        Console.WriteLine("게임을 종료합니다.");
                        return;

                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                }
            }
        }

        static void ShowCharacterStatus(float Level, string name, string job, int gold, int attack, int defence, int HP)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상태보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                Console.WriteLine("================================================================");
                Console.WriteLine($"LV. {Level}");                        //현재 기본적으로 설정되어있는 스탯에 + 표현하는 것이 힘듭니다.
                Console.WriteLine($"{name} ({job})");
                Console.WriteLine($"공격력 : {attack}");
                Console.WriteLine($"방어력 : {defence}");
                Console.WriteLine($"체력 : {HP}");
                Console.WriteLine($"Gold : {gold} G");
                Console.WriteLine("================================================================");
                Console.WriteLine("0. 나가기");
                Console.Write(">> ");
                if (Console.ReadLine() == "0")
                {
                    return;
                }
            }
        }
        static void ShowInventory(ref int attack, ref int defence, List<Item> inventory)
        {
            while (true)
            {
                Console.WriteLine("=======================================");
                Console.WriteLine(" 인벤토리");
                Console.WriteLine(" 보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("                                       ");
                Console.WriteLine(" [아이템 목록]");
                if (inventory.Count == 0)
                {
                    Console.WriteLine("인벤토리에 아이템이 없습니다.");
                }
                else
                {
                    foreach (var item in inventory)
                    {
                        string equippedStatus = item.IsEquipped ? "[E] " : "";
                        Console.WriteLine($" {equippedStatus}{item.Name} - {item.Effect} (착용 완료)");
                    }
                }
                Console.WriteLine("                                       ");
                Console.WriteLine(" 1. 장착 관리");
                Console.WriteLine(" 0. 나가기");
                Console.WriteLine("                                       ");
                Console.WriteLine(" 원하시는 행동을 입력해주세요");
                Console.Write(" >> ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        EquipItem(ref attack, ref defence, inventory);
                        break;

                    case "0":
                        Console.WriteLine(" 인벤토리에서 나갑니다.");
                        Thread.Sleep(1000);
                        return;

                    default:
                        Console.WriteLine(" 잘못된 입력입니다. 다시 시도하세요.");
                        break;
                }
            }
        }
        static void EquipItem(ref int attack, ref int defence, List<Item> inventory)
        {
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine("             장착 관리");
            Console.WriteLine("======================================");

            if (inventory.Count == 0)
            {
                Console.WriteLine(" 장착할 아이템이 없습니다.");
                return;
            }

            for (int i = 0; i < inventory.Count; i++)
            {
                string equippedStatus = inventory[i].IsEquipped ? "[E] " : "";
                Console.WriteLine($" {i + 1}. {equippedStatus}{inventory[i].Name} - {inventory[i].Effect}");
            }

            Console.WriteLine("\n 장착할 아이템의 번호를 입력하세요 (0: 나가기)");
            Console.Write(" >> ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= inventory.Count)
            {
                var selectedItem = inventory[choice - 1];

                if (selectedItem.IsEquipped)
                {
                    Console.WriteLine($" {selectedItem.Name}의 장착을 해제합니다.");
                    selectedItem.IsEquipped = false;
                    AdjustStats(ref attack, ref defence, selectedItem, false);
                }
                else
                {
                    Console.WriteLine($" {selectedItem.Name}을(를) 장착합니다.");
                    selectedItem.IsEquipped = true;
                    AdjustStats(ref attack, ref defence, selectedItem, true);
                }
            }
            else if (choice == 0)
            {
                Console.WriteLine(" 장착 관리를 종료합니다.");
            }
            else
            {
                Console.WriteLine(" 잘못된 입력입니다.");
            }

        }

        static void AdjustStats(ref int attack, ref int defence, Item item, bool equip)
        {
            int value = equip ? 1 : -1;
            if (item.Effect.Contains("공격력"))
            {
                int effectValue = ExtractStatValue(item.Effect);
                attack += effectValue * value;
            }
            else if (item.Effect.Contains("방어력"))
            {
                int effectValue = ExtractStatValue(item.Effect);
                defence += effectValue * value;
            }
        }

        static int ExtractStatValue(string effect)
        {
            string[] parts = effect.Split(' ');
            return int.Parse(parts[1].Replace("+", ""));
        }



        static void Shop(ref int gold, List<Item> inventory)
        {
            List<Item> shopItems = new List<Item>
            {
                new Item(1, "수련자 갑옷"    , "방어력 +5", "수련에 도움을 주는 갑옷입니다.",                     1000),
                new Item(2, "무쇠갑옷"       , "방어력 +9", "무쇠로 만들어져 튼튼한 갑옷입니다.",                 1500),
                new Item(3, "스파르타의 갑옷", "방어력 +15", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500),
                new Item(4, "낡은 검"        , "공격력 +2", "쉽게 볼 수 있는 낡은 검입니다.",                      600),
                new Item(5, "청동 도끼"      , "공격력 +5", "어디선가 사용됐던 거 같은 도끼입니다.",              1500),
                new Item(6, "스파르타의 창"  , "공격력 +7", "스파르타의 전사들이 사용했다는 전설의 창입니다.",    3500)
            };


            while (true)
            {
                string behavior = "";

                Console.WriteLine("=========================================");
                Console.WriteLine(" 상점 ");
                Console.WriteLine(" 필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("                                         ");
                Console.WriteLine($" [보유 골드] {gold} G");
                Console.WriteLine("                                         ");
                Console.WriteLine(" [아이템 목록]");
                Console.WriteLine(" - 수련자 갑옷      | 방어력 + 5  | 수련에 도움을 주는 갑옷입니다.                    | 1000 G");
                Console.WriteLine(" - 무쇠갑옷         | 방어력 + 9  | 무쇠로 만들어져 튼튼한 갑옷입니다.                | 1500 G");
                Console.WriteLine(" - 스파르타의 갑옷  | 방어력 + 15 | 스파르타의 전사들이 사용했다는 전설의 갑옷입니다. | 3500 G");
                Console.WriteLine(" - 낡은 검          | 공격력 + 2  | 쉽게 볼 수 있는 낡은 검 입니다.                   |  600 G");
                Console.WriteLine(" - 청동 도끼        | 공격력 + 5  | 어디선가 사용됐던거 같은 도끼입니다.              | 1500 G");
                Console.WriteLine(" - 스파르타의 창    | 공격력 + 7  | 스파르타의 전사들이 사용했다는 전설의 창입니다.   | 3500 G");

                Console.WriteLine(" 1. 아이템 구매");
                Console.WriteLine(" 0. 나가기");
                Console.WriteLine("                                         ");
                Console.WriteLine(" 원하시는 행동을 입력하세요");
                Console.WriteLine(" >> ");

                behavior = Console.ReadLine();

                switch (behavior)
                {
                    case "1":
                        foreach (var item in shopItems)
                        {
                            Console.WriteLine();
                            string status = item.Purchased ? "구매완료" : $"{item.Price} G";
                            Console.WriteLine($"{item.ID}, {item.Name} l {item.Effect} l {item.Description} | {status}");
                        }
                        break;

                    case "0":
                        Console.WriteLine("게임을 종료합니다.");
                        return;

                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                }



                PurchaseItem(shopItems, inventory, ref gold);
            }
                void PurchaseItem(List<Item> shopItems, List<Item> inventory, ref int gold)
                {
                    Console.WriteLine("원하시는 아이템의 숫자를 입력하세요 (0을 입력하면 나갑니다)");
                    Console.Write(" >> ");
                    string itemChoice = Console.ReadLine();
                    Console.Clear();

                    if (itemChoice == "0")
                        return;

                    int itemID;
                    if (int.TryParse(itemChoice, out itemID) && itemID >= 1 && itemID <= 6)
                    {
                        var item = shopItems.Find(i => i.ID == itemID);
                        if (item.Purchased)
                        {
                            Console.WriteLine("이미 구매한 아이템입니다.");

                        }
                        else if (gold >= item.Price)
                        {
                            gold -= item.Price;
                            item.Purchased = true;
                            inventory.Add(item);
                            Console.WriteLine($"구매를 완료했습니다. {item.Name}을(를) 구입했습니다. 남은 금액: {gold} G");
                        }
                        else
                        {
                            Console.WriteLine(" Gold가 부족합니다.");
                        }
                    }
                }
            }


        }
        public class Item
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Effect { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }
            public bool Purchased { get; set; }
            public bool IsEquipped { get; set; }
            public Item(int id, string name, string effect, string description, int price)
            {
                ID = id;
                Name = name;
                Effect = effect;
                Description = description;
                Price = price;
                Purchased = false;
                IsEquipped = false;
            }


        }
    }

