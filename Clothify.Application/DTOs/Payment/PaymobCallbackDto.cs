namespace Clothify.Application.DTOs.Payment
{
    public class PaymobCallbackDto
    {
        public string type { get; set; } = null!;
        public PaymobCallbackObject obj { get; set; } = null!;
    }

    public class PaymobCallbackObject
    {
        public int id { get; set; }
        public bool success { get; set; }
        public bool pending { get; set; }
        public int amount_cents { get; set; }
        public string currency { get; set; } = null!;
        public int integration_id { get; set; }
        public int owner { get; set; }
        public string created_at { get; set; } = null!;
        public bool error_occured { get; set; }
        public bool has_parent_transaction { get; set; }
        public bool is_3d_secure { get; set; }
        public bool is_auth { get; set; }
        public bool is_capture { get; set; }
        public bool is_refunded { get; set; }
        public bool is_standalone_payment { get; set; }
        public bool is_voided { get; set; }
        public PaymobCallbackOrder order { get; set; } = null!;
        public PaymobSourceData source_data { get; set; } = null!;
    }

    public class PaymobSourceData
    {
        public string pan { get; set; } = null!;
        public string sub_type { get; set; } = null!;
        public string type { get; set; } = null!;
    }

    public class PaymobCallbackOrder
    {
        public int id { get; set; }
        public string merchant_order_id { get; set; } = null!;
    }
}
