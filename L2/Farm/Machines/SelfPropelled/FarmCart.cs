using System.Collections.Generic;
using Farm.Products;

namespace Farm.Machines.SelfPropelled;


public class FarmCart : Machine
{
	private readonly List<Product> _products = new();
	public IReadOnlyList<Product> Products => _products.AsReadOnly();

	public void LoadProduct(Product product)
	{
		_products.Add(product);
	}

	public void UnloadProduct(Product product)
	{
		_products.Remove(product);
	}
}